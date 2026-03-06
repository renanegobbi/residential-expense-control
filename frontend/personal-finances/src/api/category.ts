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

/**
 * 1 = Despesa
 * 2 = Receita
 * 3 = Ambas
 */
export type CategoryPurpose = 1 | 2 | 3;

export type Category = {
  id: number;
  description: string;
  purpose: CategoryPurpose;
};

export type SearchCategoryRequest = {
  pageIndex: number;
  pageSize: number;
  orderBy: number;
  orderDirection: "ASC" | "DESC";
};

export async function searchCategories(req: SearchCategoryRequest) {
  const { data } = await http.post<ApiResponse<PagedResult<Category>>>("/category/search", req);
  return data;
}

export type RegisterCategoryRequest = {
  description: string;
  categoryPurpose: CategoryPurpose;
};

export async function registerCategory(req: RegisterCategoryRequest) {
  const { data } = await http.post<ApiResponse<Category>>("/category/register", req);
  return data;
}

export type UpdateCategoryRequest = {
  id: number;
  description: string;
  categoryPurpose: CategoryPurpose;
};

export async function updateCategory(req: UpdateCategoryRequest) {
  const { data } = await http.put<ApiResponse<Category>>("/category/update", req);
  return data;
}

export async function deleteCategory(id: number) {
  const { data } = await http.delete<ApiResponse<Category>>("/category/delete", {
    params: { id },
  });
  return data;
}