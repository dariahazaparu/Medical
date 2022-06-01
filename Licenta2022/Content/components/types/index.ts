import { ColumnGroupType, ColumnType as BaseColumnType } from "antd/lib/table";

export type TableColumnsType<RecordType> = (ColumnGroupType<RecordType> | BaseColumnType<RecordType> | {
    omitSearch?: boolean
})[]

export interface Person {
    Nume: string;
    Prenume: string;
}

export interface Pacient extends Person {
    Adresa: {
        Localitate: string;
        Strada: string;
        Numar: string;
    }
}

export interface DropdownItem {
    label: string;
    value: any;
}