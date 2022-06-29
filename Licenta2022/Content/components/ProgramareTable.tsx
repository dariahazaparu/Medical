import { Checkbox } from "antd";
import moment from "moment";
import TableComponent from "./Table"
import { TableHeader } from "./TableHeader";
import { TableColumnsType } from "./types"
import { goToRoute } from "./utils";

interface Programare {
    Id: string;

    Data: string

    Doctor: {
        Nume: string;
        Prenume: string;
    }

    Pacient: {
        Nume: string;
        Prenume: string;
    }

    NumeClinica: string;

    Prezent: boolean
}

interface DataType extends Programare {
    key: string
}

interface IProgramareTable {
    data: DataType[]
}

const columns = [
    {
        title: "Pacient",
        key: "pacient",
        width: "20%",

        render: (_, record) => record.Pacient.Nume + " " + record.Pacient.Prenume,

        sorter: (a, b) => a.Pacient.Nume.localeCompare(b.Pacient.Nume)
    },

    {
        title: "Data",
        key: "data",
        dataIndex: "Data",

        width: "20%",

        render: data => moment(data).format("dddd Do MMMM gggg HH:mm"),

        sorter: (a, b) => moment(a.Data).diff(b.Data)
    },

    {
        title: "Doctor",
        key: "numeDoctor",
        width: '20%',

        render: (_, record) => record.Doctor.Nume + " " + record.Doctor.Prenume,

        sorter: (a, b) => a.Doctor.Nume.localeCompare(b.Doctor.Nume)
    },
    {
        title: "Clinica",
        dataIndex: "NumeClinica",
        key: "numeClinica",
        width: "20%",

        sorter: (a, b) => a.NumeClinica.localeCompare(b.NumeClinica)
    },

    {
        title: "Prezent",
        dataIndex: "Prezent",
        key: "prezent",
        width: "30%",

        omitSearch: true,

        render: (value) => <Checkbox disabled checked={value} />,

        sorter: (a, b) => Number(a.Prezent) - Number(b.Prezent)
    }
] as TableColumnsType<Programare>

const ProgramareTable: React.FC<IProgramareTable> = ({ data }) => {
    return (
        <div>
            <TableHeader title="Programări" omitCreate />

            <TableComponent onDetailsClick={(id) => goToRoute(`/Programare/Details/${id}`)} data={data.map((item, index) => ({
                ...item,
                key: index.toString()
            }))} columns={columns} actions={{ omitEdit: true }} />
        </div>
    )
}

export default ProgramareTable
