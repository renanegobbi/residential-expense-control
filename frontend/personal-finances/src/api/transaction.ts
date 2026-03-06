import { http } from "./http";

export type ApiResponse<T> = {
  success: boolean;
  messages: string[];
  result: T;
};

export type PagedResult<T> = {
  pageIndex: number;
  pageSize: number;
  orderBy: string;
  orderDirection: string;
  totalRecords: number;
  totalPages: number;
  data: T[];
};

/** type:
 * 0 = Receita
 * 1 = Despesa
 */
export type TransactionType = 1 | 2;

export const TRANSACTION_TYPE = {
    Expense: 1 as TransactionType, // Despesa
    Income: 2 as TransactionType,  // Receita
  } as const;

export type Transaction = {
  id: number;
  description: string;
  amount: number;
  type: TransactionType;
  categoryId: number;
  personId: number;
};

export type SearchTransactionRequest = {
  pageIndex: number;
  pageSize: number;
  orderBy: number;
  orderDirection: "ASC" | "DESC";
};

export async function searchTransactions(req: SearchTransactionRequest) {
  const { data } = await http.post<ApiResponse<PagedResult<Transaction>>>("/transaction/search", req);
  return data;
}

export async function getTransactionById(transactionId: number) {
  const { data } = await http.get<ApiResponse<Transaction>>("/transaction/get-by-id", {
    params: { transactionId },
  });
  return data;
}

export type RegisterTransactionRequest = {
  description: string;
  amount: number;
  type: TransactionType;
  categoryId: number;
  personId: number;
};

export async function registerTransaction(req: RegisterTransactionRequest) {
  const { data } = await http.post<ApiResponse<Transaction>>("/transaction/register", req);
  return data;
}

export type UpdateTransactionRequest = {
  id: number;
  description: string;
  amount: number;
  type: TransactionType;
  categoryId: number;
  personId: number;
};

export async function updateTransaction(req: UpdateTransactionRequest) {
  const { data } = await http.put<ApiResponse<Transaction>>("/transaction/update", req);
  return data;
}

export async function deleteTransaction(id: number) {
  const { data } = await http.delete<ApiResponse<Transaction>>("/transaction/delete", {
    params: { id },
  });
  return data;
}