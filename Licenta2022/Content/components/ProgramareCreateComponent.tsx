import { Button, DatePicker, Radio, Select, TimePicker } from 'antd'
import { useState } from 'react'

import moment from 'moment'
import axios from 'axios'

interface Specialitate {
    Id: number
    Nume: string

    Doctori: Doctor[]
}

interface Doctor {
    Id: number
    Nume: string
    Prenume: string

    Programe: Program[]
}

interface Program {
    Id: number;

    Config: string;

    Data: string;
}

interface IProgramareCreateComponent {
    specialitati: Specialitate[]
    idPacient: number
    idSpecializare: number;
    idTrimitere: number;
}

const ProgramareCreateComponent: React.FC<IProgramareCreateComponent> = (props) => {
    const { specialitati, idPacient, idTrimitere, idSpecializare } = props

    console.log(props)

    const [specializareId, setSpecializareId] = useState(idSpecializare === -1 ? null : idSpecializare)
    const [doctorId, setDoctorId] = useState(null)
    const [startFromDate, setStartFromDate] = useState(null)

    const [programId, setProgramId] = useState(null)
    const [programIntervalIndex, setIntervalIndex] = useState(null)

    const [startOfDay] = useState(moment().startOf("day"))

    const specializare = specialitati.find(specialitate => specialitate.Id === specializareId)

    const doctor = specializare?.Doctori?.find(doctor => doctor.Id === doctorId)

    const doctorProgrameAvailableDates = doctor?.Programe?.map(program => moment(program.Data))

    const updateSpecializare = (specializareId: number) => {
        setSpecializareId(specializareId)

        updateDoctor(null)
    }

    const updateDoctor = (doctorId: number) => {
        setDoctorId(doctorId)

        setStartFromDate(null)

        setProgramId(null)
    }

    const onSelectInterval = (interval: number) => {
        setIntervalIndex(interval)
    }

    const onCreate = async () => {
        const response = await axios.post("/Programare/Create", {
            IdDoctor: doctorId,
            IdProgram: programId,
            ProgramIntervalIndex: programIntervalIndex,
            IdPacient: idPacient,
            IdTrimitere: idTrimitere
        })

        if (response.status === 200) {
            window.location.href = "/Programare"
        }

        console.log({ specializareId, doctorId, programId, programIntervalIndex, idPacient })
    }

    return (
        <div>
            <div>
                <h6>Specializare</h6>
                <Select disabled={props.idSpecializare !== -1} value={specializareId} style={{ minWidth: "250px" }} placeholder="Selecteaza" onChange={updateSpecializare}>
                    {specialitati.map((specializare, idx) => (
                        <Select.Option value={specializare.Id}>{specializare.Nume}</Select.Option>
                    ))}
                </Select>
            </div>

            {specializare ? (
                <div style={{ marginTop: "2rem" }}>
                    <h6>Doctor</h6>
                    <Select value={doctorId} style={{ minWidth: "250px" }} placeholder="Selecteaza" onChange={updateDoctor}>
                        {specializare.Doctori.map((doctor, idx) => (
                            <Select.Option value={doctor.Id}>{doctor.Nume} {doctor.Prenume}</Select.Option>
                        ))}
                    </Select>
                </div>
            ) : null}

            {doctor ? (
                <div style={{ marginTop: "1rem" }}>
                    <h6>Start from date:</h6>

                    <DatePicker disabledDate={date => date.isBefore(startOfDay) || !doctorProgrameAvailableDates.some(avDate => avDate.isSame(date, "day"))} value={startFromDate} onChange={setStartFromDate} />
                </div>
            ) : null}

            {startFromDate ? (
                <div style={{ marginTop: "1rem" }}>
                    {doctor.Programe.filter(program => moment(program.Data).isSameOrAfter(startFromDate.startOf("day"))).map((program, idx) => (
                        <div key={idx}>
                            <div>
                                {moment(program.Data).format("dddd Do MMMM gggg")}
                            </div>

                            {program.Config.split("").map((value, idx) => {
                                if (value === "0") return null;

                                const startHour = moment(startOfDay).add(idx * 15, "minutes").format("HH:mm")

                                return <Radio.Button style={{ marginRight: "1rem" }} onChange={e => {
                                    onSelectInterval(e.target.value)
                                    setProgramId(program.Id)
                                }} key={idx} checked={programId === program.Id && idx === programIntervalIndex} value={idx}>{startHour}</Radio.Button>
                            })}
                        </div>
                    ))}
                </div>
            ) : null}

            <Button disabled={!programIntervalIndex} style={{ marginTop: "2rem" }} onClick={onCreate}>Creeaza</Button>
        </div>
    )
}

export default ProgramareCreateComponent
