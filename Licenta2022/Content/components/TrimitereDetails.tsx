import { Button, Card, Descriptions } from "antd";
import { Person } from "./types";
import { goToRoute } from "./utils";

interface Trimitere {
    Id: number;

    Observatii: string;

    Pacient: Person & {
        Id: number;
    }

    IdProgramare: number;
    IdProgramareParinte: number;

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

const ButtonsComponent: React.FC<ITrimitereDetails> = ({ trimitere: { Pacient, Id, IdProgramare, IdProgramareParinte } }) => {
    console.log({ IdProgramare, IdProgramareParinte })

    return <Card style={{ display: "flex", flexDirection: "column" }}>
        <div style={{ display: "flex", justifyContent: "center" }}>
            <Button type="primary" onClick={() => goToRoute(`/Programare/Details/${IdProgramare}`)}>Vezi programare</Button>
        </div>

        <div style={{ marginTop: "2rem" }}>
            {IdProgramareParinte === -1 ? <Button type="primary" onClick={() => goToRoute(`/Programare/Create/${Pacient.Id}/${Id}`)}>Creează programare pe baza trimiterii</Button> :
                <Button type="primary" onClick={() => goToRoute(`/Programare/Details/${IdProgramareParinte}`)}>Vezi programarea pe baza trimiterii</Button>}

        </div>

        <div style={{ marginTop: "2rem", display: "flex", justifyContent: "center" }}>
            <Button disabled={(IdProgramareParinte !== -1)} onClick={() => goToRoute(`/Trimitere/Delete/${Id}`)} type="primary">Șterge trimiterea</Button>
        </div>
    </Card>
}


export default TrimitereDetails
