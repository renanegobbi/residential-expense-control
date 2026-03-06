import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import { FiEdit2, FiTrash2, FiPlus } from "react-icons/fi";
import AppDialog from "../components/ui/AppDialog";
import { useTableSort } from "../hooks/useTableSort";
import { FiChevronLeft, FiChevronRight, FiChevronsLeft, FiChevronsRight } from "react-icons/fi";
import {
    Category,
    CategoryPurpose,
    searchCategories,
    registerCategory,
    updateCategory,
    deleteCategory,
} from "../api/category";

function purposeLabel(p: CategoryPurpose) {
    if (p === 1) return "Despesa";
    if (p === 2) return "Receita";
    return "Ambas";
}

export default function CategoriesPage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const { sortField, sortDir, toggleSort } = useTableSort<"Id" | "Description" | "Purpose">("Id", "ASC");

    const [pageIndex, setPageIndex] = useState(1);
    const [pageSize, setPageSize] = useState(5);

    const [totalPages, setTotalPages] = useState(1);
    const [totalRecords, setTotalRecords] = useState(0);
    const [records, setRecords] = useState<Category[]>([]);

    const [showForm, setShowForm] = useState(true);
    const [editingId, setEditingId] = useState<number | null>(null);

    const [description, setDescription] = useState("");
    const [purpose, setPurpose] = useState<CategoryPurpose>(1);

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

    function showConfirm(message: string, onConfirm: () => void, title = "Confirmar exclusão") {
        setDialogTitle(title);
        setDialogMsg(message);
        setDialogVariant("warning");
        setDialogShowCancel(true);
        setDialogConfirmText("Excluir");
        setDialogCancelText("Cancelar");
        setDialogOnConfirm(() => onConfirm);
        setDialogOpen(true);
    }
    const request = useMemo(
        () => ({
            pageIndex,
            pageSize,
            orderBy: sortField,
            orderDirection: sortDir,
        }),
        [pageIndex, pageSize, sortField, sortDir]
    );

    async function load() {
        setLoading(true);
        setError(null);

        try {
            const resp = await searchCategories(request);
            if (!resp.success) {
                setError(resp.messages?.join(" | ") || "Falha na consulta.");
                return;
            }

            setRecords(resp.result.data);
            setTotalPages(resp.result.totalPages);
            setTotalRecords(resp.result.totalRecords);
        } catch (e: any) {
            setError(e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado ao chamar a API.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        void load();
    }, [request]);

    function resetForm() {
        setEditingId(null);
        setDescription("");
        setPurpose(1);
    }

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();

        if (!description.trim()) {
            showInfo("Descrição é obrigatória.", "warning");
            return;
        }

        if (description.trim().length > 400) {
            showInfo("Descrição deve ter no máximo 400 caracteres.", "warning");
            return;
        }

        try {
            setLoading(true);

            const payload = {
                description: description.trim(),
                categoryPurpose: purpose,
            };

            const resp = editingId
                ? await updateCategory({ id: editingId, ...payload })
                : await registerCategory(payload);

            if (!resp.success) {
                showInfo(resp.messages?.join(" | ") ?? "Erro ao salvar.", "danger", "Erro");
                return;
            }

            showInfo(resp.messages?.[0] ?? "Sucesso!", "success", "Sucesso");
            resetForm();
            setShowForm(true);
            await load();
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

    function onEdit(c: Category) {
        setEditingId(c.id);
        setDescription(c.description);
        setPurpose(c.purpose);
        setShowForm(true);
    }

    async function onDelete(c: Category) {
        showConfirm(`Excluir categoria "${c.description}"?`, async () => {
            try {
                setLoading(true);

                const resp = await deleteCategory(c.id);

                if (!resp.success) {
                    showInfo(resp.messages?.join(" | ") ?? "Erro ao excluir.", "danger", "Erro");
                    return;
                }

                showInfo(resp.messages?.[0] ?? "Excluído!", "success", "Sucesso");

                if (editingId === c.id) resetForm();
                await load();
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

    return (
        <Page>
            <Header>
                <TitleArea>
                    <h1>Categorias</h1>
                    <small>Total: {totalRecords}</small>
                </TitleArea>

                <Actions>
                    <button onClick={() => setShowForm((v) => !v)}>
                        <FiPlus /> {showForm ? "Ocultar" : "Nova Categoria"}
                    </button>
                    <button onClick={load} disabled={loading}>
                        Atualizar
                    </button>
                </Actions>
            </Header>

            {showForm && (
                <FormCard onSubmit={onSubmit}>
                    <FormGrid>
                        <label className="wide">
                            Descrição
                            <input
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                maxLength={400}
                                placeholder="Ex: Alimentação"
                            />
                        </label>

                        <label>
                            Finalidade
                            <select value={purpose} onChange={(e) => setPurpose(Number(e.target.value) as CategoryPurpose)}>
                                <option value={1}>Despesa</option>
                                <option value={2}>Receita</option>
                                <option value={3}>Ambas</option>
                            </select>
                        </label>

                        <FormButtons>
                            <button type="submit" disabled={loading}>
                                {editingId ? "Atualizar" : "Salvar"}
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
                <CardTitle>Categorias Cadastradas</CardTitle>

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
                                <th style={{ width: 220 }}>
                                    <ThBtn type="button" onClick={() => toggleSort("Purpose")}>
                                        Finalidade {sortField === "Purpose" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                                    </ThBtn>
                                </th>
                                <th style={{ width: 160 }}>Ações</th>
                            </tr>
                        </thead>

                        <tbody>
                            {loading ? (
                                <tr><td colSpan={4}>Carregando...</td></tr>
                            ) : records.length === 0 ? (
                                <tr><td colSpan={4}>Nenhum registro encontrado.</td></tr>
                            ) : (
                                records.map((c) => (
                                    <tr key={c.id}>
                                        <td>{c.id}</td>
                                        <td>{c.description}</td>
                                        <td>
                                            <Pill className={`p${c.purpose}`}>
                                                {purposeLabel(c.purpose)}
                                            </Pill>
                                        </td>
                                        <td>
                                            <RowActions>
                                                <IconBtn className="edit" onClick={() => onEdit(c)} title="Editar">
                                                    <FiEdit2 />
                                                </IconBtn>
                                                <IconBtn className="delete" onClick={() => void onDelete(c)} title="Excluir">
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
                        <PageNavBtn onClick={() => setPageIndex(1)} disabled={pageIndex <= 1 || loading} aria-label="Primeira página" title="Primeira">
                            <FiChevronsLeft />
                        </PageNavBtn>

                        <PageNavBtn onClick={() => setPageIndex((p) => Math.max(1, p - 1))} disabled={pageIndex <= 1 || loading} aria-label="Página anterior" title="Anterior">
                            <FiChevronLeft />
                        </PageNavBtn>

                        <span className="info">
                            Página <b>{pageIndex}</b> de <b>{totalPages}</b>
                        </span>

                        <PageNavBtn onClick={() => setPageIndex((p) => Math.min(totalPages, p + 1))} disabled={pageIndex >= totalPages || loading} aria-label="Próxima página" title="Próxima">
                            <FiChevronRight />
                        </PageNavBtn>

                        <PageNavBtn onClick={() => setPageIndex(totalPages)} disabled={pageIndex >= totalPages || loading} aria-label="Última página" title="Última">
                            <FiChevronsRight />
                        </PageNavBtn>
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
  h1 { margin: 0; font-size: 22px; margin-bottom: 5px;}
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
  grid-template-columns: 1fr 220px auto;
  gap: 12px;
  align-items: end;

  .wide { grid-column: 1 / 2; }

  @media (max-width: 900px) { grid-template-columns: 1fr 1fr; .wide { grid-column: 1 / -1; } }
  @media (max-width: 600px) { grid-template-columns: 1fr; }

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

const FormButtons = styled.div`
  display: flex;
  gap: 10px;
  justify-content: flex-end;

  @media (max-width: 900px) { grid-column: 1 / -1; justify-content: flex-start; }

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

  table { width: 100%; border-collapse: collapse; min-width: 720px; }
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

const Pill = styled.span`
  display: inline-flex;
  align-items: center;
  padding: 6px 10px;
  border-radius: 999px;
  font-weight: 800;
  font-size: 14px;

  &.p1 { background: rgba(239,68,68,0.12); color: #991b1b; }  /* despesa */
  &.p2 { background: rgba(22,163,74,0.12); color: #166534; }   /* receita */
  &.p3 { background: rgba(59,130,246,0.12); color: #1d4ed8; }   /* ambas */
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

const PageNavBtn = styled.button`
  width: 36px;
  height: 36px;
  border-radius: 999px;
  border: 1px solid #d0d5dd;
  background: #fff;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;

  svg { font-size: 18px; }

  transition: transform 0.08s ease, box-shadow 0.15s ease, background 0.15s ease;

  &:hover:not(:disabled) {
    background: #f8fafc;
    box-shadow: 0 8px 20px rgba(15, 23, 42, 0.10);
    transform: translateY(-1px);
  }

  &:active:not(:disabled) {
    transform: translateY(0px);
    box-shadow: 0 4px 10px rgba(15, 23, 42, 0.08);
  }

  &:disabled {
    opacity: 0.45;
    cursor: not-allowed;
    box-shadow: none;
  }

  &:focus-visible {
    outline: none;
    box-shadow: 0 0 0 3px rgba(14, 165, 233, 0.25);
  }
`;