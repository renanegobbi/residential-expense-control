import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import { FiEdit2, FiTrash2, FiPlus } from "react-icons/fi";
import AppDialog from "../components/ui/AppDialog";

import { Person, searchPeople } from "../api/people";
import { Category, searchCategories } from "../api/category";
import { useTableSort } from "../hooks/useTableSort";
import {
    Transaction,
    TransactionType,
    searchTransactions,
    registerTransaction,
    updateTransaction,
    deleteTransaction,
    TRANSACTION_TYPE,
} from "../api/transaction";

function typeLabel(t: TransactionType) {
    return t === TRANSACTION_TYPE.Income ? "Receita" : "Despesa";
}

function purposeAllowsType(purpose: number, type: TransactionType) {
    if (purpose === 3) return true; // ambas
    if (type === TRANSACTION_TYPE.Income) return purpose === 2; // receita
    return purpose === 1; // despesa
}

export default function TransactionsPage() {
    const [orderBy, setOrderBy] = useState("Id");
    const [orderDirection, setOrderDirection] = useState<"ASC" | "DESC">("ASC");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const { sortField, sortDir, toggleSort } = useTableSort<"Id" | "Description" | "Type" | "Amount">("Id", "ASC");

    const [pageIndex, setPageIndex] = useState(1);
    const [pageSize, setPageSize] = useState(5);
    const [totalPages, setTotalPages] = useState(1);
    const [totalRecords, setTotalRecords] = useState(0);
    const [records, setRecords] = useState<Transaction[]>([]);

    const [people, setPeople] = useState<Person[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);

    const [showForm, setShowForm] = useState(true);
    const [editingId, setEditingId] = useState<number | null>(null);

    const [personId, setPersonId] = useState<number | "">("");
    const [type, setType] = useState<TransactionType>(TRANSACTION_TYPE.Expense);
    const [categoryId, setCategoryId] = useState<number | "">("");
    const [description, setDescription] = useState("");
    const [amount, setAmount] = useState<number | "">("");

    const [dialogOpen, setDialogOpen] = useState(false);
    const [dialogMsg, setDialogMsg] = useState("");
    const [dialogTitle, setDialogTitle] = useState("Aviso");
    const [dialogVariant, setDialogVariant] =
        useState<"info" | "success" | "warning" | "danger">("info");

    const [dialogShowCancel, setDialogShowCancel] = useState(false);
    const [dialogConfirmText, setDialogConfirmText] = useState("OK");
    const [dialogCancelText, setDialogCancelText] = useState("Cancelar");
    const [dialogOnConfirm, setDialogOnConfirm] = useState<null | (() => void)>(null);

    function showInfo(
        message: string,
        variant: "info" | "success" | "warning" | "danger" = "info",
        title = "Aviso"
    ) {
        setDialogTitle(title);
        setDialogMsg(message);
        setDialogVariant(variant);
        setDialogShowCancel(false);
        setDialogConfirmText("OK");
        setDialogCancelText("Cancelar");
        setDialogOnConfirm(null);
        setDialogOpen(true);
    }

    function showConfirm(
        message: string,
        onConfirm: () => void,
        title = "Confirmar exclusão"
    ) {
        setDialogTitle(title);
        setDialogMsg(message);
        setDialogVariant("warning");
        setDialogShowCancel(true);
        setDialogConfirmText("Excluir");
        setDialogCancelText("Cancelar");
        setDialogOnConfirm(() => onConfirm);
        setDialogOpen(true);
    }

    const selectedPerson = useMemo(
        () => people.find((p) => p.id === personId) ?? null,
        [people, personId]
    );

    const filteredCategories = useMemo(() => {
        return categories.filter((c) => purposeAllowsType(c.purpose, type));
    }, [categories, type]);

    const request = useMemo(
        () => ({
            pageIndex,
            pageSize,
            orderBy: sortField,
            orderDirection: sortDir,
        }),
        [pageIndex, pageSize, sortField, sortDir]
    );

    function changeSort(column: string) {

        if (orderBy === column) {
            setOrderDirection(prev => prev === "ASC" ? "DESC" : "ASC");
        } else {
            setOrderBy(column);
            setOrderDirection("ASC");
        }

    }

    async function loadAll() {
        setLoading(true);
        setError(null);

        try {
            const tr = await searchTransactions(request);
            if (!tr.success) {
                setError(tr.messages?.join(" | ") || "Falha ao buscar transações.");
                return;
            }
            setRecords(tr.result.data);
            setTotalPages(tr.result.totalPages);
            setTotalRecords(tr.result.totalRecords);

            const pe = await searchPeople({
                id: null,
                name: null,
                age: null,
                minAge: null,
                maxAge: null,
                pageIndex: 1,
                pageSize: 200,
                orderBy: "Id",
                orderDirection: "ASC",
            });
            if (pe.success) setPeople(pe.result.data);

            const ca = await searchCategories({
                pageIndex: 1,
                pageSize: 200,
                orderBy: "Id",
                orderDirection: "ASC",
            });
            if (ca.success) setCategories(ca.result.data);
        } catch (e: any) {
            setError(e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        void loadAll();
    }, [request]);

    useEffect(() => {
        if (categoryId === "") return;
        const cat = categories.find((c) => c.id === categoryId);
        if (!cat) return;
        if (!purposeAllowsType(cat.purpose, type)) setCategoryId("");
    }, [type, categories, categoryId]);

    useEffect(() => {
        if (!selectedPerson) return;
        if (selectedPerson.age < 18 && type === TRANSACTION_TYPE.Income) {
            setType(TRANSACTION_TYPE.Expense);
        }
    }, [selectedPerson, type]);

    function resetForm() {
        setEditingId(null);
        setPersonId("");
        setType(TRANSACTION_TYPE.Expense);
        setCategoryId("");
        setDescription("");
        setAmount("");
    }

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();

        if (personId === "") return showInfo("Selecione uma pessoa.", "warning");
        if (!description.trim()) return showInfo("Descrição é obrigatória.", "warning");
        if (amount === "" || Number.isNaN(Number(amount)) || Number(amount) <= 0)
            return showInfo("Valor deve ser positivo.", "warning");
        if (categoryId === "") return showInfo("Selecione uma categoria.", "warning");

        const person = people.find((p) => p.id === personId);

        if (person && person.age < 18 && type === TRANSACTION_TYPE.Income) {
            return showInfo("Menor de idade: apenas DESPESA é permitida.", "warning");
        }

        const cat = categories.find((c) => c.id === categoryId);

        if (cat && !purposeAllowsType(cat.purpose, type)) {
            return showInfo("Categoria inválida para este tipo (Receita/Despesa).", "warning");
        }

        try {
            setLoading(true);

            const payload = {
                description: description.trim(),
                amount: Number(amount),
                type,
                categoryId: Number(categoryId),
                personId: Number(personId),
            };

            const resp = editingId
                ? await updateTransaction({ id: editingId, ...payload })
                : await registerTransaction(payload);

            if (!resp.success) {
                showInfo(resp.messages?.join(" | ") ?? "Erro ao salvar.", "danger", "Erro");
                return;
            }

            showInfo(resp.messages?.[0] ?? "Sucesso!", "success", "Sucesso");
            resetForm();
            await loadAll();
        } catch (e: any) {
            showInfo(
                e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado.",
                "danger",
                "Erro"
            );
        } finally {
            setLoading(false);
        }
    }

    function normalizeTransactionType(raw: number): TransactionType {
        if (raw === 1 || raw === 2) return raw as TransactionType;

        if (raw === 1) return TRANSACTION_TYPE.Income;
        if (raw === 2) return TRANSACTION_TYPE.Expense;

        return TRANSACTION_TYPE.Expense;
    }

    function onEdit(t: Transaction) {
        setEditingId(t.id);
        setShowForm(true);
        setPersonId(t.personId);
        setType(normalizeTransactionType(t.type));
        setCategoryId(t.categoryId);
        setDescription(t.description);
        setAmount(t.amount);
    }

    async function onDelete(t: Transaction) {
        showConfirm(`Excluir transação ID = ${t.id}?`, async () => {
            try {
                setLoading(true);
                const resp = await deleteTransaction(t.id);

                if (!resp.success) {
                    showInfo(resp.messages?.join(" | ") ?? "Erro ao excluir.", "danger", "Erro");
                    return;
                }

                showInfo(resp.messages?.[0] ?? "Excluído!", "success", "Sucesso");

                if (editingId === t.id) resetForm();
                await loadAll();
            } catch (e: any) {
                showInfo(
                    e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado.",
                    "danger",
                    "Erro"
                );
            } finally {
                setLoading(false);
            }
        });
    }

    const personName = (id: number) => people.find((p) => p.id === id)?.name ?? `#${id}`;
    const categoryName = (id: number) => categories.find((c) => c.id === id)?.description ?? `#${id}`;

    return (
        <Page>
            <Header>
                <TitleArea>
                    <h1>Transações</h1>
                    <small>Total: {totalRecords}</small>
                </TitleArea>

                <Actions>
                    <button onClick={() => setShowForm((v) => !v)}>
                        <FiPlus /> {showForm ? "Ocultar" : "Nova Transação"}
                    </button>
                    <button onClick={() => void loadAll()} disabled={loading}>
                        Atualizar
                    </button>
                </Actions>
            </Header>

            {showForm && (
                <FormCard onSubmit={onSubmit}>
                    <FormGrid>
                        <label>
                            Pessoa
                            <select value={personId} onChange={(e) => setPersonId(e.target.value === "" ? "" : Number(e.target.value))}>
                                <option value="">Selecione...</option>
                                {people.map((p) => (
                                    <option key={p.id} value={p.id}>
                                        {p.name} - ({p.age} anos)
                                    </option>
                                ))}
                            </select>
                        </label>

                        <TypeBox>
                            <span>Tipo</span>
                            <TypeButtons>
                                <button
                                    type="button"
                                    className={type === TRANSACTION_TYPE.Income ? "active ok" : "ghost"}
                                    onClick={() => {
                                        if (selectedPerson && selectedPerson.age < 18) {
                                            showInfo("Menor de idade: apenas DESPESA é permitida.", "warning");
                                            setType(TRANSACTION_TYPE.Expense);
                                            return;
                                        }
                                        setType(TRANSACTION_TYPE.Income);
                                    }}
                                >
                                    Receita
                                </button>

                                <button
                                    type="button"
                                    className={type === TRANSACTION_TYPE.Expense ? "active bad" : "ghost"}
                                    onClick={() => setType(TRANSACTION_TYPE.Expense)}
                                >
                                    Despesa
                                </button>
                            </TypeButtons>
                        </TypeBox>

                        <label>
                            Categoria
                            <select value={categoryId} onChange={(e) => setCategoryId(e.target.value === "" ? "" : Number(e.target.value))}>
                                <option value="">Selecione...</option>
                                {filteredCategories.map((c) => (
                                    <option key={c.id} value={c.id}>
                                        {c.description}
                                    </option>
                                ))}
                            </select>
                        </label>

                        <label className="wide">
                            Descrição
                            <input
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                maxLength={400}
                                placeholder="Ex: Pagamento conta de luz"
                            />
                        </label>

                        <label>
                            Valor
                            <input
                                type="number"
                                step="0.01"
                                min={0}
                                value={amount}
                                onChange={(e) => setAmount(e.target.value === "" ? "" : Number(e.target.value))}
                                placeholder="Ex: 150,75"
                            />
                        </label>

                        <FormButtons>
                            <button type="submit" disabled={loading}>
                                {editingId ? "Atualizar" : "Cadastrar"}
                            </button>

                            <button
                                type="button"
                                className="secondary"
                                onClick={resetForm}
                                disabled={loading}
                            >
                                Limpar
                            </button>
                        </FormButtons>
                    </FormGrid>
                </FormCard>
            )}

            {error && <ErrorBox>{error}</ErrorBox>}

            <Card>
                <CardTitle>Transações Cadastradas</CardTitle>

                <TableWrap>
                    <table>
                        <thead>
                            <tr>
                                <th style={{ width: 80 }}>
                                    <ThBtn type="button" onClick={() => toggleSort("Id")}>
                                        ID {sortField === "Id" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                                    </ThBtn>
                                </th>
                                <th>
                                    <ThBtn type="button" onClick={() => toggleSort("Description")}>
                                        Descrição {sortField === "Description" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                                    </ThBtn>
                                </th>
                                <th style={{ width: 130 }}>
                                    <ThBtn type="button" onClick={() => toggleSort("Type")}>
                                        Tipo {sortField === "Type" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                                    </ThBtn>
                                </th>
                                <th style={{ width: 140 }}>
                                    <ThBtn type="button" onClick={() => toggleSort("Amount")}>
                                        Valor {sortField === "Amount" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                                    </ThBtn>
                                </th>
                                <th style={{ width: 200 }}>Pessoa</th>
                                <th style={{ width: 220 }}>Categoria</th>
                                <th style={{ width: 160 }}>Ação</th>
                            </tr>
                        </thead>

                        <tbody>
                            {loading ? (
                                <tr><td colSpan={7}>Carregando...</td></tr>
                            ) : records.length === 0 ? (
                                <tr><td colSpan={7}>Nenhuma transação encontrada.</td></tr>
                            ) : (
                                records.map((t) => (
                                    <tr key={t.id}>
                                        <td>{t.id}</td>
                                        <td>{t.description}</td>
                                        <td>
                                            <TypePill className={t.type === TRANSACTION_TYPE.Income ? "ok" : "bad"}>
                                                {typeLabel(t.type)}
                                            </TypePill>
                                        </td>
                                        <td>{t.amount.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</td>
                                        <td>{personName(t.personId)}</td>
                                        <td>{categoryName(t.categoryId)}</td>
                                        <td>
                                            <RowActions>
                                                <IconBtn className="edit" onClick={() => onEdit(t)} title="Editar">
                                                    <FiEdit2 />
                                                </IconBtn>
                                                <IconBtn className="delete" onClick={() => void onDelete(t)} title="Excluir">
                                                    <FiTrash2 />
                                                </IconBtn>
                                            </RowActions>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </TableWrap>

                <Footer>
                    <div>
                        <button onClick={() => setPageIndex(1)} disabled={pageIndex <= 1 || loading}>{"<<"}</button>
                        <button onClick={() => setPageIndex((p) => Math.max(1, p - 1))} disabled={pageIndex <= 1 || loading}>{"<"}</button>

                        <span className="info">
                            Página <b>{pageIndex}</b> de <b>{totalPages}</b>
                        </span>

                        <button onClick={() => setPageIndex((p) => Math.min(totalPages, p + 1))} disabled={pageIndex >= totalPages || loading}>{">"}</button>
                        <button onClick={() => setPageIndex(totalPages)} disabled={pageIndex >= totalPages || loading}>{">>"}</button>
                    </div>

                    <label className="size">
                        Tamanho:
                        <select
                            value={pageSize}
                            onChange={(e) => { setPageIndex(1); setPageSize(Number(e.target.value)); }}
                            disabled={loading}
                        >
                            {[5, 10, 20, 50, 100, 200].map((n) => <option key={n} value={n}>{n}</option>)}
                        </select>
                    </label>
                </Footer>
            </Card>
            <AppDialog
                open={dialogOpen}
                title={dialogTitle}
                message={dialogMsg}
                variant={dialogVariant}
                showCancel={dialogShowCancel}
                confirmText={dialogConfirmText}
                cancelText={dialogCancelText}
                onClose={() => {
                    setDialogOpen(false);
                    setDialogOnConfirm(null);
                }}
                onConfirm={() => {
                    const fn = dialogOnConfirm;
                    setDialogOpen(false);
                    setDialogOnConfirm(null);
                    fn?.();
                }}
            />
        </Page>
    );
}

const Page = styled.div`
  display: flex;
  flex-direction: column;
  gap: 12px;
`;

const Header = styled.div`
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  gap: 12px;

  @media (max-width: 700px) {
    flex-direction: column;
    align-items: stretch;
  }
`;

const TitleArea = styled.div`
  h1 { margin: 0; font-size: 22px; margin-bottom: 5px; }
  small { color: #667085; font-size: 16px; font-weight: 500; }
`;

const Actions = styled.div`
  display: flex;
  gap: 10px;

  @media (max-width: 700px) { justify-content: space-between; }

  button {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    border: 1px solid #d0d5dd;
    background: white;
    padding: 9px 12px;
    border-radius: 12px;
    cursor: pointer;
  }
  button:disabled { opacity: 0.6; cursor: not-allowed; }
`;

const FormCard = styled.form`
  background: white;
  border: 1px solid #e7e9f0;
  border-radius: 14px;
  padding: 14px;
`;

const FormGrid = styled.div`
  display: grid;
  grid-template-columns: 1.2fr 1fr 1.2fr 200px auto;
  gap: 12px;
  align-items: end;

  .wide { grid-column: 1 / -3; }

  @media (max-width: 1050px) {
    grid-template-columns: 1fr 1fr;
    .wide { grid-column: 1 / -1; }
  }

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
  }

  label {
    display: flex;
    flex-direction: column;
    gap: 6px;
    font-size: 14px;
    color: #667085;
  }

  input, select {
    height: 40px;
    border: 1px solid #d0d5dd;
    border-radius: 12px;
    padding: 0 12px;
    font-size: 14px;
    outline: none;
    background: white;
  }

  input:focus, select:focus {
    border-color: #6aa6ff;
    box-shadow: 0 0 0 3px rgba(106,166,255,0.18);
  }
`;

const TypeBox = styled.div`
  display: flex;
  flex-direction: column;
  gap: 6px;
  font-size: 14px;
  color: #667085;
`;

const TypeButtons = styled.div`
  display: flex;
  gap: 10px;

  button {
    border: 1px solid #d0d5dd;
    background: white;
    padding: 9px 12px;
    border-radius: 12px;
    cursor: pointer;
    font-weight: 700;
    font-size: 14px;
  }

  button.active.ok { background: #16a34a; border-color: #16a34a; color: white; }
  button.active.bad { background: #ef4444; border-color: #ef4444; color: white; }
`;

const FormButtons = styled.div`
  display: flex;
  gap: 10px;
  justify-content: flex-end;

  @media (max-width: 1050px) {
    grid-column: 1 / -1;
    justify-content: flex-start;
  }

  button {
    border: 0;
    padding: 10px 14px;
    border-radius: 12px;
    cursor: pointer;
    color: white;
    background: #0ea5e9;
    font-weight: 700;
  }

  button.secondary { background: #6b7280; }
`;

const Card = styled.div`
  background: white;
  border: 1px solid #e7e9f0;
  border-radius: 14px;
  padding: 14px;
`;

const CardTitle = styled.h3`
  margin: 0 0 12px 0;
  text-align: center;
`;

const TableWrap = styled.div`
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;

  table { width: 100%; border-collapse: collapse; min-width: 980px; }
  th, td { border-bottom: 1px solid #eef0f5; padding: 12px; text-align: left; }
  th { font-size: 14px; color: #667085; font-weight: 800; background: #f8fafc; }
`;

const RowActions = styled.div`
  display: flex;
  gap: 10px;
`;

const IconBtn = styled.button`
  border: 0;
  cursor: pointer;
  border-radius: 10px;
  padding: 10px 12px;
  display: inline-flex;
  align-items: center;
  justify-content: center;

  &.edit { background: #f59e0b; color: #111827; }
  &.delete { background: #ef4444; color: white; }

  svg { font-size: 16px; }
`;

const TypePill = styled.span`
  display: inline-flex;
  align-items: center;
  padding: 6px 10px;
  border-radius: 999px;
  font-weight: 800;
  font-size: 14px;

  &.ok { background: rgba(22,163,74,0.12); color: #166534; }
  &.bad { background: rgba(239,68,68,0.12); color: #991b1b; }
`;

const Footer = styled.div`
  margin-top: 12px;
  display: flex;
  justify-content: space-between;
  gap: 12px;
  align-items: center;
  flex-wrap: wrap;

  button {
    border: 1px solid #d0d5dd;
    background: white;
    padding: 7px 10px;
    border-radius: 10px;
    cursor: pointer;
    margin-right: 6px;
  }

  .info { margin: 0 10px; font-size: 13px; }
  .size { display: flex; gap: 8px; align-items: center; font-size: 13px; color: #374151; }
  select { padding: 7px 10px; border-radius: 10px; border: 1px solid #d0d5dd; }
`;

const ErrorBox = styled.div`
  background: #fff1f1;
  border: 1px solid #ffd0d0;
  color: #b42318;
  padding: 10px;
  border-radius: 12px;
`;

const ThBtn = styled.button`
  background: transparent;
  border: 0;
  padding: 0;
  font: inherit;
  color: inherit;
  cursor: pointer;
  font-weight: 800;
`;