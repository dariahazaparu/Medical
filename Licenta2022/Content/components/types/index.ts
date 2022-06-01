import { ColumnGroupType, ColumnType as BaseColumnType } from "antd/lib/table";

export type TableColumnsType<RecordType> = (ColumnGroupType<RecordType> | BaseColumnType<RecordType> | {
    omitSearch?: boolean
})[]

export interface Person {
    Nume: string;
    Prenume: string;
}