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

    RetetaId: number
    FacturaId: number;
}

interface IProgramareDetails {
    programare: Programare;
    servicii: {
        Pret: number;
        Denumire: string
    }[]

    isPacient: boolean;
}

const ProgramareDetails: React.FC<IProgramareDetails> = ({ programare, servicii, isPacient }) => {
    const { Data, TrimitereId, RetetaId, FacturaId, Id, Diagnostic: { Id: DiagnosticId, Denumire: DenumireDiagnostic }, Doctor, Clinica, TrimitereTId, Pacient } = programare

    const [facturaId, setFacturaId] = useState(FacturaId)

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

    const onCreateFactura = async () => {
        try {
            const response = await axios.post("/Factura/Create", {
                IdProgramare: Id
            })

            notification.success({ message: "Ai generat factura cu success!" })

            setFacturaId(response.data.FacturaId)
        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }

    const onDeleteProgramare = async () => {
        goToRoute(`/Programare/Delete/${programare.Id}`)
    }

    return (
        <div>
            <h3>Detalii</h3>
            <div >
                <div>
                    <b>Prezent</b>: <Input disabled={isPacient || prezent} title="Prezent" type="checkbox" checked={prezent} onChange={e => onChangePrezent(e.target.checked)} />
                </div>

                {prezent && (
                    <>
                        <hr />
                        <div style={{ display: "flex", justifyContent: "space-around" }}>
                            <div>
                                {TrimitereId !== -1 ? <Button type="primary" onClick={() => goToRoute(`/Trimitere/Details/${TrimitereId}`)}>Vezi trimitere</Button> : (!isPacient && <Button type="primary" onClick={() => goToRoute(`/Trimitere/Create/${Id}`)}>Creează o trimitere</Button>)}
                            </div>

                            <div>
                                {facturaId === -1 ? (!isPacient && <Button type="primary" onClick={onCreateFactura}>Genereaza factura</Button>) : <Button type="primary" onClick={() => goToRoute(`/Factura/Details/${facturaId}`)}>Vezi factura</Button>}
                            </div>

                            <div>
                                {DiagnosticId === -1 ? !isPacient && (<Button type="primary" onClick={() => goToRoute(`/Pacient/AddDiagnostic/${programare.Id}`)}>Creează un diagnostic</Button>) : null}
                            </div>

                            {DiagnosticId === -1 ? null : (
                                <div>
                                    {RetetaId !== -1 ? <Button type="primary" onClick={() => goToRoute(`/Reteta/Details/${RetetaId}`)}>Vezi reteta</Button> : (!isPacient && <Button type="primary" onClick={() => goToRoute(`/Reteta/Create/${Id}`)}>Creează reteta</Button>)}
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
                    <b>Data</b>: {moment(Data).format("dddd Do MMMM gggg HH:mm")}
                </div>

                <hr />

                <div>
                    <b>Doctor</b>: {Doctor.Nume} {Doctor.Prenume}
                </div>

                <hr />

                <div>
                    <b>Pacient</b>: {Pacient.Nume} {Pacient.Prenume}
                </div>

                <hr />

                <div>
                    <b>Clinica</b>: {Clinica.Nume}
                    <div style={{ marginLeft: "1rem" }}>
                        <b>Adresa</b>: {Clinica.Adresa}

                    </div>
                </div>

                {servicii.length > 0 && (
                    <div>
                        <hr />

                        <b>Servicii</b>:  {servicii.map(serviciu => serviciu.Denumire).join(", ")}
                    </div>
                )}

                {TrimitereTId !== -1 && (
                    <div>
                        <hr />

                        <Button style={{ marginBottom: "1rem" }} onClick={() => goToRoute(`/Trimitere/Details/${TrimitereTId}`)}>Vezi trimiterea pe baza căreia a fost creată programarea</Button>
                    </div>
                )}

                <Button disabled={prezent} onClick={onDeleteProgramare}>Șterge programarea</Button>
            </div>
        </div>
    )
}

export default ProgramareDetails
