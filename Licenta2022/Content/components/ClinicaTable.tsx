import { TableColumnsType } from "antd";
import TableComponent from "./Table";
import { TableHeader } from "./TableHeader";

interface Clinica {
    Nume: string;
    Adresa: string;
}

interface DataType extends Clinica {
    key: string
}

interface IClinicaTable {
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
        title: "Adresa",
        dataIndex: "Adresa",
        key: "adresa",
        width: "30%",

        omitSearch: true,

        sorter: (a, b) => a.Adresa.localeCompare(b.Adresa)
    }

] as TableColumnsType<DataType>

const ClinicaTable: React.FC<IClinicaTable> = ({ data }) => {
    return (<div>
        <TableHeader title="Clinici" />

        <TableComponent columns={columns} data={data.map((item, idx) => ({
            ...item,
            key: idx.toString()
        }))} />
    </div>)
}

export default ClinicaTable