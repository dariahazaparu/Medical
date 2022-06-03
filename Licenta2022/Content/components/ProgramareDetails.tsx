import { Button, Input, notification } from "antd"
import axios from "axios";
import moment from "moment";
import { useState } from "react";
import { Person } from "./types"
import { goToRoute } from "./utils"

interface Programare {
    Id: number;

    Prezent: boolean;

    Pacient: Person

    Data: string

    TrimitereId: number;

    Diagnostic: {
        Id: number;
        Denumire: string;
    }

    Doctor: Person;

    TrimitereTId: number;

    Clinica: {
        Nume: string;
        Adresa: string;
    }

    Servicii: {
        Pret: number;
        Denumire: string
    }[]

    ReteteId: number[]
}

interface IProgramareDetails {
    programare: Programare
}

const ProgramareDetails: React.FC<IProgramareDetails> = ({ programare }) => {
    const { Data, TrimitereId, Id, Diagnostic: { Id: DiagnosticId, Denumire: DenumireDiagnostic }, Doctor, Clinica, Servicii, TrimitereTId } = programare
    // TODO: fix 1-1 relation
    const retetaId = programare.ReteteId[0] || -1
    const [prezent, setPrezent] = useState(programare.Prezent)

    const onChangePrezent = async (val: boolean) => {
        try {
            const response = await axios.post("/Programare/SetPrezent", {
                ProgramareId: programare.Id,
                Prezent: val
            })

            if (response.status === 200) {
                setPrezent(val)
            }

            notification.success({ message: "Prezenta a fost modificata cu succes." })
        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }

    return (
        <div>
            <div >
                <div>
                    <b>Prezent</b>: <Input title="Prezent" type="checkbox" checked={prezent} onChange={e => onChangePrezent(e.target.checked)} />
                </div>

                {prezent && (
                    <>
                        <hr />
                        <div style={{ display: "flex", justifyContent: "space-around" }}>
                            <div>
                                {TrimitereId !== -1 ? <Button type="primary" onClick={() => goToRoute(`/Trimitere/Details/${TrimitereId}`)}>Vezi trimitere</Button> : <Button type="primary" onClick={() => goToRoute(`/Trimitere/Create/${Id}`)}>Creeaza o trimitere</Button>}
                            </div>

                            <div>
                                {DiagnosticId === -1 ? <Button type="primary" onClick={() => goToRoute(`/Pacient/AddDiagnostic/${programare.Id}`)}>Creeaza un diagnostic</Button> : null}
                            </div>

                            {DiagnosticId === -1 ? null : (
                                <div>
                                    {retetaId !== -1 ? <Button type="primary" onClick={() => goToRoute(`/Reteta/Details/${retetaId}`)}>Vezi reteta</Button> : <Button type="primary" onClick={() => goToRoute(`/Reteta/Create/${Id}`)}>Creeaza reteta</Button>}
                                </div>
                            )}

                        </div>
                    </>
                )}

                {DiagnosticId === -1 ? null : (
                    <div>
                        <hr />
                        <b>Diagnostic</b>: {DenumireDiagnostic}
                    </div>
                )}

                <hr />


                <div>
                    <b>Data</b>: {moment(Data).format("dddd Do MMMM gggg")}
                </div>

                <hr />

                <div>
                    <b>Doctor</b>: {Doctor.Nume} {Doctor.Prenume}
                </div>

                <hr />

                <div>
                    <b>Clinica</b>: {Clinica.Nume}
                    <div style={{ marginLeft: "1rem" }}>
                        <b>Adresa</b>: {Clinica.Adresa}

                    </div>
                </div>

                {Servicii.length > 0 && (
                    <div>
                        <hr />

                        <b>Servicii</b>:  {Servicii.map(serviciu => serviciu.Denumire).join(", ")}
                    </div>
                )}

                {TrimitereTId !== -1 && (
                    <div>
                        <hr />

                        <Button onClick={() => goToRoute(`/Trimitere/Details/${TrimitereTId}`)}>Vezi trimiterea mama</Button>
                    </div>
                )}
            </div>
        </div>
    )
}

export default ProgramareDetails
