import TableComponent from "./Table";
import { TableHeader } from "./TableHeader";
import { Person, TableColumnsType } from "./types"

interface Doctor extends Person {
    Specializare: string;
    Clinica: string;
}

interface DataType extends Doctor {
    key: string
}

interface IDoctorTable {
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
        title: "Specializare",
        dataIndex: "Specializare",
        key: "specializare",
        width: "20%",

        omitSearch: true,

        sorter: (a, b) => a.Specializare.localeCompare(b.Specializare)
    },
    {
        title: "Clinica",
        key: "clinica",
        dataIndex: "Clinica",
        width: "20%",

        omitSearch: true,

        sorter: (a, b) => a.Clinica.localeCompare(b.Clinica)
    },

] as TableColumnsType<DataType>

const DoctorTable: React.FC<IDoctorTable> = ({ data }) => {
    return (<div>
        <TableHeader title="Doctori" />

        <TableComponent columns={columns} data={data.map((item, idx) => ({
            ...item,
            key: idx.toString()
        }))} />
    </div>)
}

export default DoctorTable