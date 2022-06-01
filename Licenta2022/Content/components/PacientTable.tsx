import { Button, Layout, Typography } from "antd";
import TableComponent from "./Table"
import { TableHeader } from "./TableHeader";
import { Pacient, Person, TableColumnsType } from "./types";
import { addToRoute } from "./utils";



interface DataType extends Pacient {
    key: string
}

interface IPacientTable {
    data: DataType[]
}

const columns = [
    {
        title: 'Nume',
        dataIndex: 'Nume',
        key: 'name',
        width: '30%',
        sorter: (a, b) => a.Nume.localeCompare(b.Nume)
    },
    {
        title: "Prenume",
        dataIndex: "Prenume",
        key: "prenume",
        width: "30%",
        sorter: (a, b) => a.Prenume.localeCompare(b.Prenume)

    },
    {
        title: "Localitate",
        dataIndex: ["Adresa", "Localitate"],
        key: "localitate",
        width: "20%",

        omitSearch: true,

        sorter: (a, b) => a.Adresa.Localitate.localeCompare(b.Adresa.Localitate)
    },
    {
        title: "Adresa",
        key: "strada",
        dataIndex: ["Adresa", "Strada"],
        width: "20%",

        omitSearch: true,

        render: (_, record) => record.Adresa.Strada + " " + record.Adresa.Numar,

        sorter: (a, b) => a.Adresa.Strada.localeCompare(b.Adresa.Strada)
    },

] as TableColumnsType<DataType>

const PacientTable: React.FC<IPacientTable> = ({ data }) => {
    return (
        <div>
            <TableHeader title="Pacienti" />

            <TableComponent columns={columns} data={data.map((item, idx) => ({
                ...item,
                key: idx.toString()
            }))} />
        </div>
    )
}

export default PacientTable
