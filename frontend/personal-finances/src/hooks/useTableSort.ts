import { useState } from "react";

export type SortDir = "ASC" | "DESC";

export function useTableSort<TField extends string>(
  initialField: TField,
  initialDir: SortDir = "ASC"
) {
  const [sortField, setSortField] = useState<TField>(initialField);
  const [sortDir, setSortDir] = useState<SortDir>(initialDir);

  function toggleSort(field: TField) {
    if (field === sortField) {
      setSortDir((d) => (d === "ASC" ? "DESC" : "ASC"));
    } else {
      setSortField(field);
      setSortDir("ASC");
    }
  }

  return { sortField, sortDir, toggleSort, setSortField, setSortDir };
}