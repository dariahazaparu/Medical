import { Button, Descriptions } from "antd";
import moment from "moment";
import { useState } from "react";
import { goToRoute } from "./utils";

interface Configuratie {
    Config: string;
    Data: string;
}

interface Doctor {
    Id: string;
    Nume: string;
    Prenume: string;
    Specializare: string;
    Clinica: string;
    Configuratii: Configuratie[];
}

interface IDoctorDetails {
    doctor: Doctor
    isDeletable: boolean
}

const DoctorDetails: React.FC<IDoctorDetails> = ({ doctor, isDeletable }) => {
    const { Id, Nume, Prenume, Specializare, Clinica, Configuratii } = doctor

    const [startOfDay] = useState(moment().startOf("day"))

    return (
        <div>
            <Descriptions bordered title="Doctor">
                <Descriptions.Item label="Nume">{Nume}</Descriptions.Item>
                <Descriptions.Item label="Prenume">{Prenume}</Descriptions.Item>
                <Descriptions.Item label="Specializare">{Specializare}</Descriptions.Item>
                <Descriptions.Item label="Clinica">{Clinica}</Descriptions.Item>
            </Descriptions>

            <div style={{ marginTop: "2rem" }}>
                {Configuratii.length == 0 ? <h6>Nu există nicio configurație, momentan.</h6> : (
                    <div>
                        <h3>Program:</h3>
                        <hr />
                        <div>
                            {Configuratii.map((configuratie, index) => {
                                const startIndex = configuratie.Config.indexOf("1")
                                const endIndex = configuratie.Config.lastIndexOf("1") + 1

                                let text = ""

                                if (endIndex - startIndex == 96) {
                                    text = "Toată ziua"
                                }

                                else {
                                    const startHour = moment(startOfDay).add(startIndex * 15, "minutes")
                                    const endHour = moment(startOfDay).add(endIndex * 15, "minutes")

                                    text = `${startHour.format("HH:mm")} - ${endHour.format("HH:mm")}`
                                }

                                return <div key={index}>
                                    <h6>Data:</h6> {moment(configuratie.Data).format("dddd Do MMMM gggg")}
                                    <h6>Interval:</h6> {text}
                                    <hr />
                                </div>
                            })}
                        </div>
                    </div>
                )}
                <Button type="primary" onClick={() => goToRoute(`/Doctor/Program/${Id}`)}>Adaugă program</Button>
                <Button disabled={!isDeletable} style={{ marginLeft: "1rem" }} onClick={() => goToRoute(`/Doctor/Delete/${Id}`)}>Șterge doctor</Button>
            </div>
        </div>
    )
}

export default DoctorDetails
