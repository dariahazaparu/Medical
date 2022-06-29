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
    omitCreate: boolean
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

const ClinicaTable: React.FC<IClinicaTable> = ({ data, omitCreate }) => {
    return (<div>
        <TableHeader title="Clinici" {...{ omitCreate }} />

        <TableComponent columns={columns} data={data.map((item, idx) => ({
            ...item,
            key: idx.toString()
        }))} actions={{ omitEdit: omitCreate }} />
    </div>)
}

export default ClinicaTable