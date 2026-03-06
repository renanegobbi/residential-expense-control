import { http } from "./http";

export type Person = {
  id: number;
  name: string;
  age: number;
};

export type SearchPeopleRequest = {
  pageIndex: number;
  pageSize: number;
  orderBy: string;
  orderDirection: "ASC" | "DESC";
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

export type ApiResponse<T> = {
  success: boolean;
  messages: string[];
  result: T;
};

// SEARCH
export async function searchPeople(req: SearchPeopleRequest) {
  const { data } = await http.post<ApiResponse<PagedResult<Person>>>("/people/search", req);
  return data;
}

export type CreatePersonRequest = {
  name: string;
  age: number;
};

export async function createPerson(req: CreatePersonRequest) {
  const { data } = await http.post<ApiResponse<Person>>("/create", req);
  return data;
}

// REGISTER
export type RegisterPersonRequest = {
  name: string;
  age: number;
};

export async function registerPerson(req: RegisterPersonRequest) {
  const { data } = await http.post<ApiResponse<Person>>("/people/register", req);
  return data;
}

// UPDATE 
export type UpdatePersonRequest = {
  id: number;
  name: string;
  age: number;
};

export async function updatePerson(req: UpdatePersonRequest) {
  const { data } = await http.put<ApiResponse<Person>>("/people/update", req);
  return data;
}

// DELETE
export async function deletePerson(id: number) {
  const { data } = await http.delete<ApiResponse<Person>>(`/people/delete`, {
    params: { id },
  });
  return data;
}