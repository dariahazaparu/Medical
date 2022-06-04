import { Button, Card, Descriptions } from "antd";
import { Person } from "./types";
import { goToRoute } from "./utils";

interface Trimitere {
    Observatii: string;

    Pacient: Person;

    ProgramareId: number;
    ProgramareTId: number;

    Servicii: string[];
    Specializare: string;
}

interface ITrimitereDetails {
    trimitere: Trimitere
}

const TrimitereDetails: React.FC<ITrimitereDetails> = ({ trimitere }) => {
    const { Observatii, Pacient, Servicii, Specializare } = trimitere

    return (
        <Descriptions extra={<ButtonsComponent {...{ trimitere }} />} bordered title="Trimitere" layout="vertical">
            <Descriptions.Item label="Pacient">{Pacient.Nume} {Pacient.Prenume}</Descriptions.Item>
            <Descriptions.Item label="Observatii">{Observatii}</Descriptions.Item>
            <Descriptions.Item label="Specializare">{Specializare}</Descriptions.Item>
            <Descriptions.Item label="Servicii">{Servicii.join(", ")}</Descriptions.Item>
        </Descriptions>
    )
}

const ButtonsComponent: React.FC<ITrimitereDetails> = ({ trimitere: { ProgramareId, ProgramareTId } }) => <Card style={{ display: "flex", flexDirection: "column" }}>
    <div style={{ display: "flex", justifyContent: "center" }}>
        <Button type="primary" onClick={() => goToRoute(`/Programare/Details/${ProgramareId}`)}>Vezi programare</Button>
    </div>
    {ProgramareTId != -1 && <div style={{ marginTop: "2rem" }}>
        <Button type="primary" onClick={() => goToRoute(`/Programare/Details/${ProgramareTId}`)}>Vezi programarea pe baza trimiterii</Button></div>}
</Card>

export default TrimitereDetails
