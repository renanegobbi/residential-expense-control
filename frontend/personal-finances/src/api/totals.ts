import { http } from "./http";

export type OrderDirection = "ASC" | "DESC";

export type ApiResponse<T> = {
  success: boolean;
  messages: string[];
  result: T;
};

export type TotalsSummary = {
  totalIncome: number;
  totalExpense: number;
  balance: number;
};

export type PagedTotalsResult<T> = {
  pageIndex: number;
  pageSize: number;
  orderBy: string;
  orderDirection: OrderDirection;
  totalRecords: number;
  totalPages: number;
  data: T[];
  summary: TotalsSummary;
};

export type TotalsRequest = {
  pageIndex: number;
  pageSize: number;
  orderBy: number;
  orderDirection: OrderDirection;
};

export type PeopleTotalsRow = {
  personId: number;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
};

export type CategoryTotalsRow = {
  categoryId: number;
  categoryDescription: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
};

export async function searchTotalsByPeople(req: TotalsRequest) {
  const { data } = await http.post<ApiResponse<PagedTotalsResult<PeopleTotalsRow>>>(
    "/totals/by-people",
    req
  );
  return data;
}

export async function searchTotalsByCategory(req: TotalsRequest) {
  const { data } = await http.post<ApiResponse<PagedTotalsResult<CategoryTotalsRow>>>(
    "/totals/by-category",
    req
  );
  return data;
}