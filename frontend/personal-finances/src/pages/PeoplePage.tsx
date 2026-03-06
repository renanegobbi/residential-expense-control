import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import { FiEdit2, FiTrash2, FiPlus } from "react-icons/fi";
import { searchPeople, Person, registerPerson, updatePerson, deletePerson } from "../api/people";
import AppDialog from "../components/ui/AppDialog";
import { useTableSort } from "../hooks/useTableSort";

export default function PeoplePage() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const { sortField, sortDir, toggleSort } = useTableSort<"Id" | "Name" | "Age">("Id", "ASC");

  const [pageIndex, setPageIndex] = useState(1);
  const [pageSize, setPageSize] = useState(5);

  const [totalPages, setTotalPages] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);
  const [records, setRecords] = useState<Person[]>([]);

  const [showForm, setShowForm] = useState(true);
  const [name, setName] = useState("");
  const [age, setAge] = useState<number | "">("");

  const [editingId, setEditingId] = useState<number | null>(null);

  const [dialogOpen, setDialogOpen] = useState(false);
  const [dialogMsg, setDialogMsg] = useState("");
  const [dialogTitle, setDialogTitle] = useState("Aviso");
  const [dialogVariant, setDialogVariant] = useState<"info" | "success" | "warning" | "danger">("info");
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [personToDelete, setPersonToDelete] = useState<Person | null>(null);

  function showDialog(message: string, variant: typeof dialogVariant = "info", title = "Aviso") {
    setDialogTitle(title);
    setDialogMsg(message);
    setDialogVariant(variant);
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
      const resp = await searchPeople(request);
      if (!resp.success) {
        setError(resp.messages?.join(" | ") || "Falha na consulta.");
        return;
      }

      setRecords(resp.result.data);
      setTotalPages(resp.result.totalPages);
      setTotalRecords(resp.result.totalRecords);
    } catch (e: any) {
      setError(e?.message ?? "Erro inesperado ao chamar a API.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void load();
  }, [request]);

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!name.trim()) {
      showDialog("Nome é obrigatório.", "warning");
      return;
    }

    if (age === "" || Number.isNaN(Number(age)) || Number(age) < 0) {
      showDialog("Idade inválida.", "warning");
      return;
    }

    try {
      setLoading(true);

      const payloadName = name.trim();
      const payloadAge = Number(age);

      const response = editingId
        ? await updatePerson({ id: editingId, name: payloadName, age: payloadAge })
        : await registerPerson({ name: payloadName, age: payloadAge });

      if (!response.success) {
        showDialog(response.messages?.join(" | ") ?? "Erro ao salvar.", "danger", "Erro");
        return;
      }

      showDialog(
        response.messages?.[0] ?? (editingId ? "Atualizado!" : "Cadastrado!"),
        "success",
        "Sucesso"
      );

      setEditingId(null);
      setName("");
      setAge("");
      setShowForm(true);

      await load();
    } catch (err: any) {
      showDialog(
        err?.response?.data?.messages?.join(" | ") ?? err?.message ?? "Erro inesperado.",
        "danger",
        "Erro"
      );
    } finally {
      setLoading(false);
    }
  }

  function onEdit(p: Person) {
    setEditingId(p.id);
    setName(p.name);
    setAge(p.age);
    setShowForm(true);
  }

  function onDelete(p: Person) {
    setPersonToDelete(p);
    setConfirmOpen(true);
  }

  async function confirmDeletePerson() {
    const p = personToDelete;
    if (!p) return;

    try {
      setLoading(true);

      const resp = await deletePerson(p.id);

      if (!resp.success) {
        showDialog(resp.messages?.join(" | ") ?? "Erro ao excluir.", "danger", "Erro");
        return;
      }

      showDialog(resp.messages?.[0] ?? "Excluído!", "success", "Sucesso");

      if (editingId === p.id) {
        setEditingId(null);
        setName("");
        setAge("");
      }

      await load();
    } catch (err: any) {
      showDialog(
        err?.response?.data?.messages?.join(" | ") ?? err?.message ?? "Erro inesperado.",
        "danger",
        "Erro"
      );
    } finally {
      setLoading(false);
      setConfirmOpen(false);
      setPersonToDelete(null);
    }
  }

  return (
    <Page>
      <Header>
        <TitleArea>
          <h1>Pessoas</h1>
          <small>Total: {totalRecords}</small>
        </TitleArea>

        <Actions>
          <button onClick={() => setShowForm((v) => !v)}>
            <FiPlus /> {showForm ? "Ocultar" : "Nova Pessoa"}
          </button>
          <button onClick={load} disabled={loading}>
            Atualizar
          </button>
        </Actions>
      </Header>

      {showForm && (
        <FormCard onSubmit={onSubmit}>
          <FormGrid>
            <label>
              Nome
              <input value={name} onChange={(e) => setName(e.target.value)} maxLength={200} placeholder="Ex: Lucas Fernandes" />
            </label>

            <label>
              Idade
              <input
                type="number"
                value={age}
                onChange={(e) => setAge(e.target.value === "" ? "" : Number(e.target.value))}
                min={0}
                placeholder="Ex: 33"
              />
            </label>

            <FormButtons>
              <button type="submit" disabled={loading}>
                {editingId ? "Atualizar" : "Salvar"}
              </button>

              <button
                type="button"
                className="secondary"
                onClick={() => {
                  setEditingId(null);
                  setName("");
                  setAge("");
                }}
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
        <CardTitle>Pessoas Cadastradas</CardTitle>

        <TableWrap>
          <table>
            <thead>
              <tr>
                <th style={{ width: 80 }}>
                  <ThBtn type="button" onClick={() => toggleSort("Id")}>
                    ID {sortField === "Id" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                  </ThBtn>
                </th>
                <th >
                  <ThBtn type="button" onClick={() => toggleSort("Name")}>
                    Nome {sortField === "Name" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                  </ThBtn>
                </th>

                <th style={{ width: 220 }}>
                  <ThBtn type="button" onClick={() => toggleSort("Age")}>
                    Idade {sortField === "Age" ? (sortDir === "ASC" ? "▲" : "▼") : ""}
                  </ThBtn>
                </th>
                <th style={{ width: 140 }}>Ações</th>
              </tr>
            </thead>

            <tbody>
              {loading ? (
                <tr><td colSpan={4}>Carregando...</td></tr>
              ) : records.length === 0 ? (
                <tr><td colSpan={4}>Nenhum registro encontrado.</td></tr>
              ) : (
                records.map((p) => (
                  <tr key={p.id}>
                    <td>{p.id}</td>
                    <td>{p.name}</td>
                    <td>{p.age}</td>
                    <td>
                      <RowActions>
                        <IconBtn className="edit" onClick={() => onEdit(p)} title="Editar">
                          <FiEdit2 />
                        </IconBtn>
                        <IconBtn className="delete" onClick={() => void onDelete(p)} title="Excluir">
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
        onClose={() => setDialogOpen(false)}
      />
      <AppDialog
        open={confirmOpen}
        title="Confirmar exclusão"
        message={
          personToDelete
            ? `Tem certeza que deseja excluir "${personToDelete.name}"?\n\n(Isso apaga transações também)`
            : ""
        }
        variant="danger"
        confirmText="Excluir"
        cancelText="Cancelar"
        showCancel
        onClose={() => {
          setConfirmOpen(false);
          setPersonToDelete(null);
        }}
        onConfirm={confirmDeletePerson}
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
  h1 { margin: 0; font-size: 22px; margin-bottom: 5px;  }
  small { color: #667085; font-size: 16px; font-weight: 500; }
`;

const Actions = styled.div`
  display: flex;
  gap: 10px;

  @media (max-width: 700px) {
    justify-content: space-between;
  }

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

  @media (max-width: 900px) {
    grid-template-columns: 1fr 1fr;
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

  input {
    height: 40px;
    border: 1px solid #d0d5dd;
    border-radius: 12px;
    padding: 0 12px;
    font-size: 14px;
    outline: none;
    background: white;
  }

  input:focus {
    border-color: #6aa6ff;
    box-shadow: 0 0 0 3px rgba(106,166,255,0.18);
  }
`;

const FormButtons = styled.div`
  display: flex;
  gap: 10px;
  justify-content: flex-end;

  @media (max-width: 900px) {
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
    font-weight: 600;
  }

  button.secondary {
    background: #6b7280;
  }
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

  table { width: 100%; border-collapse: collapse; min-width: 640px; }
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