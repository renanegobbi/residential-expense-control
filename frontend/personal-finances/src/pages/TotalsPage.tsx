import { useEffect, useMemo, useState } from "react";
import { useTableSort } from "../hooks/useTableSort";
import styled from "styled-components";
import {
    CategoryTotalsRow,
    PeopleTotalsRow,
    TotalsSummary,
    searchTotalsByCategory,
    searchTotalsByPeople,
} from "../api/totals";

type OrderDirection = "ASC" | "DESC";

const TotalsPersonOrderBy = {
  PersonId: 0,
  PersonName: 1,
  TotalIncome: 2,
  TotalExpense: 3,
  Balance: 4,
} as const;

const TotalsCategoryOrderBy = {
  CategoryId: 0,
  CategoryDescription: 1,
  TotalIncome: 2,
  TotalExpense: 3,
  Balance: 4,
} as const;

const money = (v: number) =>
    (v ?? 0).toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

const moneyParenIfNegative = (v: number) => {
    const n = v ?? 0;
    if (n < 0) return `( ${money(Math.abs(n))} )`;
    return money(n);
};

const expenseParen = (v: number) => `( ${money(v ?? 0)} )`;

export default function TotalsPage() {

    const peopleSort = useTableSort<keyof typeof TotalsPersonOrderBy>("PersonId", "ASC");
    const catSort = useTableSort<keyof typeof TotalsCategoryOrderBy>("CategoryId", "ASC");

    const [showPeople, setShowPeople] = useState(true);
    const [loadingPeople, setLoadingPeople] = useState(false);
    const [errorPeople, setErrorPeople] = useState<string | null>(null);
    const [peopleRows, setPeopleRows] = useState<PeopleTotalsRow[]>([]);
    const [peopleSummary, setPeopleSummary] = useState<TotalsSummary | null>(null);

    const [showCategory, setShowCategory] = useState(true);
    const [loadingCategory, setLoadingCategory] = useState(false);
    const [errorCategory, setErrorCategory] = useState<string | null>(null);
    const [catRows, setCatRows] = useState<CategoryTotalsRow[]>([]);
    const [catSummary, setCatSummary] = useState<TotalsSummary | null>(null);

    const [peoplePageIndex, setPeoplePageIndex] = useState(1);
    const [peoplePageSize, setPeoplePageSize] = useState(5);
    const [peopleTotalPages, setPeopleTotalPages] = useState(1);
    const [peopleTotalRecords, setPeopleTotalRecords] = useState(0);

    const [catPageIndex, setCatPageIndex] = useState(1);
    const [catPageSize, setCatPageSize] = useState(5);
    const [catTotalPages, setCatTotalPages] = useState(1);
    const [catTotalRecords, setCatTotalRecords] = useState(0);

    const peopleRequest = useMemo(() => ({
        pageIndex: peoplePageIndex,
        pageSize: peoplePageSize,
        orderBy: TotalsPersonOrderBy[peopleSort.sortField],
        orderDirection: peopleSort.sortDir,
    }), [peoplePageIndex, peoplePageSize, peopleSort.sortField, peopleSort.sortDir]);

    const catRequest = useMemo(() => ({
        pageIndex: catPageIndex,
        pageSize: catPageSize,
        orderBy: TotalsCategoryOrderBy[catSort.sortField],
        orderDirection: catSort.sortDir,
    }), [catPageIndex, catPageSize, catSort.sortField, catSort.sortDir]);

    async function loadPeople() {
        setLoadingPeople(true);
        setErrorPeople(null);

        try {
            const r = await searchTotalsByPeople(peopleRequest);

            if (!r.success) {
                setErrorPeople(r.messages?.join(" | ") || "Falha ao consultar totais por pessoa.");
                return;
            }

            setPeopleRows(r.result.data);
            setPeopleSummary(r.result.summary);
            setPeopleTotalPages(r.result.totalPages);
            setPeopleTotalRecords(r.result.totalRecords);
        } catch (e: any) {
            setErrorPeople(e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado.");
        } finally {
            setLoadingPeople(false);
        }
    }

    async function loadCategory() {
        setLoadingCategory(true);
        setErrorCategory(null);

        try {
            const r = await searchTotalsByCategory(catRequest);

            if (!r.success) {
                setErrorCategory(r.messages?.join(" | ") || "Falha ao consultar totais por categoria.");
                return;
            }

            setCatRows(r.result.data);
            setCatSummary(r.result.summary);
            setCatTotalPages(r.result.totalPages);
            setCatTotalRecords(r.result.totalRecords);
        } catch (e: any) {
            setErrorCategory(e?.response?.data?.messages?.join(" | ") ?? e?.message ?? "Erro inesperado.");
        } finally {
            setLoadingCategory(false);
        }
    }

    useEffect(() => {
        void loadPeople();
    }, [peopleRequest]);

    useEffect(() => {
        void loadCategory();
    }, [catRequest]);

    async function loadAll() {
        await Promise.all([loadPeople(), loadCategory()]);
    }

    function PaginationFooter(props: {
        pageIndex: number;
        totalPages: number;
        totalRecords: number;
        pageSize: number;
        loading: boolean;
        onFirst: () => void;
        onPrev: () => void;
        onNext: () => void;
        onLast: () => void;
        onChangePageSize: (n: number) => void;
    }) {
        const {
            pageIndex, totalPages, totalRecords, pageSize, loading,
            onFirst, onPrev, onNext, onLast, onChangePageSize
        } = props;

        return (
            <Footer>
                <div>
                    <button onClick={onFirst} disabled={pageIndex <= 1 || loading}>{"<<"}</button>
                    <button onClick={onPrev} disabled={pageIndex <= 1 || loading}>{"<"}</button>

                    <span className="info">
                        Página <b>{pageIndex}</b> de <b>{totalPages}</b> — Total: <b>{totalRecords}</b>
                    </span>

                    <button onClick={onNext} disabled={pageIndex >= totalPages || loading}>{">"}</button>
                    <button onClick={onLast} disabled={pageIndex >= totalPages || loading}>{">>"}</button>
                </div>

                <label className="size">
                    Tamanho:
                    <select
                        value={pageSize}
                        onChange={(e) => onChangePageSize(Number(e.target.value))}
                        disabled={loading}
                    >
                        {[5, 10, 25, 50, 100].map(n => <option key={n} value={n}>{n}</option>)}
                    </select>
                </label>
            </Footer>
        );
    }


    return (
        <Page>
            <Header>
                <TitleArea>
                    <h1>Totais</h1>
                    <small>Receitas, despesas e saldo</small>
                </TitleArea>

                <Actions>
                    <button onClick={() => void loadAll()} disabled={loadingPeople || loadingCategory}>
                        Atualizar
                    </button>
                </Actions>
            </Header>

            <Card>
                <CardHeader>
                    <h3>Totais por Pessoa</h3>

                    <CardActions>
                        <button onClick={() => setShowPeople((v) => !v)}>
                            {showPeople ? "Ocultar" : "Mostrar"}
                        </button>
                        <button onClick={() => void loadPeople()} disabled={loadingPeople}>
                            Atualizar
                        </button>
                    </CardActions>
                </CardHeader>

                {errorPeople && <ErrorBox>{errorPeople}</ErrorBox>}

                {showPeople && (
                    <TableWrap>
                        <table>
                            <thead>
                                <tr>
                                    <th style={{ width: 90 }}>
                                        <ThBtn type="button" onClick={() => peopleSort.toggleSort("PersonId")}>
                                            ID {peopleSort.sortField === "PersonId" ? (peopleSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th>
                                        <ThBtn type="button" onClick={() => peopleSort.toggleSort("PersonName")}>
                                            Pessoa {peopleSort.sortField === "PersonName" ? (peopleSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => peopleSort.toggleSort("TotalIncome")}>
                                            Receitas {peopleSort.sortField === "TotalIncome" ? (peopleSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => peopleSort.toggleSort("TotalExpense")}>
                                            Despesas {peopleSort.sortField === "TotalExpense" ? (peopleSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => peopleSort.toggleSort("Balance")}>
                                            Saldo {peopleSort.sortField === "Balance" ? (peopleSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                </tr>
                            </thead>

                            <tbody>
                                {loadingPeople ? (
                                    <tr><td colSpan={5}>Carregando...</td></tr>
                                ) : peopleRows.length === 0 ? (
                                    <tr><td colSpan={5}>Nenhum registro.</td></tr>
                                ) : (
                                    <>
                                        {peopleRows.map((r) => (
                                            <tr key={r.personId}>
                                                <td>{r.personId}</td>
                                                <td>{r.personName}</td>
                                                <td>{money(r.totalIncome)}</td>
                                                <td className="neg">{expenseParen(r.totalExpense)}</td>
                                                <td className={r.balance < 0 ? "neg" : "pos"}>{moneyParenIfNegative(r.balance)}</td>
                                            </tr>
                                        ))}

                                        <tr className="summary">
                                            <td colSpan={2}><b>Total Geral</b></td>
                                            <td><b>{money(peopleSummary?.totalIncome ?? 0)}</b></td>
                                            <td className="neg"><b>{expenseParen(peopleSummary?.totalExpense ?? 0)}</b></td>
                                            <td className={(peopleSummary?.balance ?? 0) < 0 ? "neg" : "pos"}>
                                                <b>{moneyParenIfNegative(peopleSummary?.balance ?? 0)}</b>
                                            </td>
                                        </tr>
                                    </>
                                )}
                            </tbody>
                        </table>
                    </TableWrap>
                )}
                <PaginationFooter
                    pageIndex={peoplePageIndex}
                    totalPages={peopleTotalPages}
                    totalRecords={peopleTotalRecords}
                    pageSize={peoplePageSize}
                    loading={loadingPeople}
                    onFirst={() => setPeoplePageIndex(1)}
                    onPrev={() => setPeoplePageIndex(p => Math.max(1, p - 1))}
                    onNext={() => setPeoplePageIndex(p => Math.min(peopleTotalPages, p + 1))}
                    onLast={() => setPeoplePageIndex(peopleTotalPages)}
                    onChangePageSize={(n) => { setPeoplePageIndex(1); setPeoplePageSize(n); }}
                />
            </Card>

            <Card>
                <CardHeader>
                    <h3>Totais por Categoria</h3>

                    <CardActions>
                        <button onClick={() => setShowCategory((v) => !v)}>
                            {showCategory ? "Ocultar" : "Mostrar"}
                        </button>
                        <button onClick={() => void loadCategory()} disabled={loadingCategory}>
                            Atualizar
                        </button>
                    </CardActions>
                </CardHeader>

                {errorCategory && <ErrorBox>{errorCategory}</ErrorBox>}

                {showCategory && (
                    <TableWrap>
                        <table>
                            <thead>
                                <tr>
                                    <th style={{ width: 110 }}>
                                        <ThBtn type="button" onClick={() => catSort.toggleSort("CategoryId")}>
                                            ID {catSort.sortField === "CategoryId" ? (catSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th>
                                        <ThBtn type="button" onClick={() => catSort.toggleSort("CategoryDescription")}>
                                            Categoria {catSort.sortField === "CategoryDescription" ? (catSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => catSort.toggleSort("TotalIncome")}>
                                            Receitas {catSort.sortField === "TotalIncome" ? (catSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => catSort.toggleSort("TotalExpense")}>
                                            Despesas {catSort.sortField === "TotalExpense" ? (catSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                    <th style={{ width: 180 }}>
                                        <ThBtn type="button" onClick={() => catSort.toggleSort("Balance")}>
                                            Saldo {catSort.sortField === "Balance" ? (catSort.sortDir === "ASC" ? "▲" : "▼") : ""}
                                        </ThBtn>
                                    </th>
                                </tr>
                            </thead>

                            <tbody>
                                {loadingCategory ? (
                                    <tr><td colSpan={5}>Carregando...</td></tr>
                                ) : catRows.length === 0 ? (
                                    <tr><td colSpan={5}>Nenhum registro.</td></tr>
                                ) : (
                                    <>
                                        {catRows.map((r) => (
                                            <tr key={r.categoryId}>
                                                <td>{r.categoryId}</td>
                                                <td>{r.categoryDescription}</td>
                                                <td>{money(r.totalIncome)}</td>
                                                <td className="neg">{expenseParen(r.totalExpense)}</td>
                                                <td className={r.balance < 0 ? "neg" : "pos"}>{moneyParenIfNegative(r.balance)}</td>
                                            </tr>
                                        ))}

                                        <tr className="summary">
                                            <td colSpan={2}><b>Total Geral</b></td>
                                            <td><b>{money(catSummary?.totalIncome ?? 0)}</b></td>
                                            <td className="neg"><b>{expenseParen(catSummary?.totalExpense ?? 0)}</b></td>
                                            <td className={(catSummary?.balance ?? 0) < 0 ? "neg" : "pos"}>
                                                <b>{moneyParenIfNegative(catSummary?.balance ?? 0)}</b>
                                            </td>
                                        </tr>
                                    </>
                                )}
                            </tbody>
                        </table>
                    </TableWrap>
                )}
                <PaginationFooter
                    pageIndex={catPageIndex}
                    totalPages={catTotalPages}
                    totalRecords={catTotalRecords}
                    pageSize={catPageSize}
                    loading={loadingCategory}
                    onFirst={() => setCatPageIndex(1)}
                    onPrev={() => setCatPageIndex(p => Math.max(1, p - 1))}
                    onNext={() => setCatPageIndex(p => Math.min(catTotalPages, p + 1))}
                    onLast={() => setCatPageIndex(catTotalPages)}
                    onChangePageSize={(n) => { setCatPageIndex(1); setCatPageSize(n); }}
                />
            </Card>
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

const Card = styled.div`
  background: white;
  border: 1px solid #e7e9f0;
  border-radius: 14px;
  padding: 14px;
`;

const CardHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  margin-bottom: 12px;

  h3 { margin: 0; text-align: left; }

  @media (max-width: 700px) {
    flex-direction: column;
    align-items: stretch;
  }
`;

const CardActions = styled.div`
  display: flex;
  gap: 10px;

  @media (max-width: 700px) { justify-content: space-between; }

  button {
    border: 1px solid #d0d5dd;
    background: white;
    padding: 9px 12px;
    border-radius: 12px;
    cursor: pointer;
  }
  button:disabled { opacity: 0.6; cursor: not-allowed; }
`;

const TableWrap = styled.div`
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;

  table { width: 100%; border-collapse: collapse; min-width: 860px; }
  th, td { border-bottom: 1px solid #eef0f5; padding: 12px; text-align: left; }
  th { font-size: 14px; color: #667085; font-weight: 800; background: #f8fafc; }

  td.neg { color: #b42318; font-weight: 700; }
  td.pos { color: #166534; font-weight: 700; }

  tr.summary td {
    background: #f8fafc;
    border-top: 2px solid #eef0f5;
  }
`;

const ErrorBox = styled.div`
  background: #fff1f1;
  border: 1px solid #ffd0d0;
  color: #b42318;
  padding: 10px;
  border-radius: 12px;
  margin-bottom: 12px;
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

const ThBtn = styled.button`
  background: transparent;
  border: 0;
  padding: 0;
  font: inherit;
  color: inherit;
  cursor: pointer;
  font-weight: 800;
`;