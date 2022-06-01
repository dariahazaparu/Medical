import { Button, Typography } from "antd";
import { addToRoute } from "./utils";

interface ITableHeader {
    title: string;

    omitCreate?: boolean
}

export const TableHeader: React.FC<ITableHeader> = ({ title, omitCreate }) => {
    return (
        <div style={{ display: "flex", justifyContent: "space-between" }}>
            <Typography.Title level={2}>{title}</Typography.Title>
            {omitCreate ? null : <Button type="primary" style={{ marginTop: "1rem" }} onClick={() => addToRoute("/Create")}>Create</Button>}
        </div>

    )
}