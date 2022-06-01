import moment from "moment";
import TableComponent from "./Table";
import { TableHeader } from "./TableHeader";
import { Person, TableColumnsType } from "./types";

interface Trimitere {
    Id: string;

    Pacient: Person;

    DataProgramare: string;

    Specializare: string;
}

interface DataType extends Trimitere {
    key: string
}

interface ITrimitereTable {
    data: DataType[]
}

const columns = [
    {
        title: "Pacient",
        key: "pacient",
        width: "30%",

        render: (_, record) => record.Pacient.Nume + " " + record.Pacient.Prenume,
    },

    {
        title: "Data",
        key: "data",
        dataIndex: "DataProgramare",
        width: "30%",

        render: (value) => moment(value).format("dddd Do MMMM gggg"),
        sorter: (a, b) => moment(a.DataProgramare).diff(b.DataProgramare)
    },

    {
        title: "Specializare",
        key: "specializare",
        dataIndex: "Specializare",
        width: "30%",

        sorter: (a, b) => a.Specializare.localeCompare(b.Specializare)
    }
] as TableColumnsType<Trimitere>

const TrimitereTable: React.FC<ITrimitereTable> = ({ data }) => {
    console.log({ Ceplm: true })
    return (
        <div>
            <TableHeader title="Trimiteri" omitCreate />

            <TableComponent columns={columns} data={data.map((item, idx) => ({
                ...item,
                key: idx.toString()
            }))} />
        </div>

    )
}

export default TrimitereTable
