import moment from "moment";
import TableComponent from "./Table";
import { TableHeader } from "./TableHeader";
import { TableColumnsType } from "./types";

interface ProgramTemplate {
    Config: string;
}

interface DataType extends ProgramTemplate {
    key: string
}

interface IProgramTemplateTable {
    data: DataType[]
}

const startOfDay = moment().startOf("day")

const columns = [
    {
        title: "Configuratie",
        dataIndex: "Config",
        key: "config",
        width: "50%",
        render: (Config) => {

            const startIndex = Config.indexOf("1")
            const endIndex = Config.lastIndexOf("1") + 1

            let text = ""

            if (endIndex - startIndex == 96) {
                text = "Toată ziua"
            }

            else {
                const startHour = moment(startOfDay).add(startIndex * 15, "minutes")
                const endHour = moment(startOfDay).add(endIndex * 15, "minutes")

                text = `${startHour.format("HH:mm")} - ${endHour.format("HH:mm")}`
            }

            return text
        }
    }
] as TableColumnsType<DataType>

const ProgramTemplateTable: React.FC<IProgramTemplateTable> = ({ data }) => {
    return (
        <div>
            <TableHeader title="Șabloane programări" />

            <TableComponent omitActions columns={columns} data={data.map((item, idx) => ({
                ...item,
                key: idx.toString()
            }))} />
        </div>
    )
}

export default ProgramTemplateTable
