import { Button, DatePicker, Select } from 'antd';
import React, { useState } from 'react'

import moment, { Moment } from 'moment'

import { randomBytes } from 'crypto-browserify'
import axios from 'axios';

interface IDoctorProgram {
    date: Moment
    templateId: number

    id: string
}

interface ITemplate {
    Id: number;
    Config: string;
}

interface IDoctorProgramTemplateCreateComponent {
    doctorId: number
    templates: ITemplate[]
}

const DoctorProgramTemplateCreateComponent: React.FC<IDoctorProgramTemplateCreateComponent> = (props) => {
    const [programe, setPrograme] = useState<IDoctorProgram[]>([])

    const [startOfDay] = useState(moment().startOf("day"))

    const { doctorId, templates } = props

    const updateProgramTemplate = (index: number, templateId: number) => {
        console.log({ templateId })

        setPrograme(prev => {
            prev[index].templateId = templateId;

            return prev
        })
    }

    const updateProgramDate = (index: number, date: Moment) => {
        setPrograme(prev => {
            prev[index].date = date

            return prev
        })
    }

    const addProgram = () => {
        setPrograme(prev => prev.concat({
            templateId: null,
            date: null,

            id: randomBytes(16).toString()
        }))
    }

    const stergeProgram = (idx: number) => {
        setPrograme(prev => {
            const programe = Object.assign([], prev)

            programe.splice(idx, 1)

            return programe
        })
    }

    const onCreate = async () => {
        const response = await axios.post("/Doctor/Program", {
            IdDoctor: doctorId,

            Programe: programe.map(program => ({
                Data: program.date.startOf("day").toDate(),
                IdProgramTemplate: program.templateId
            })
            )
        })

        console.log({ response })

        // if (response.status === 200) {
        //     window.location.href = `/mai-tarziu`
        // }
    }

    console.log({ templates })

    return (
        <div>
            {programe.length === 0 ? <h6>Nu ai niciun program creat. :D</h6> : null}

            <div style={{ marginBottom: "2rem" }}>
                {programe.map((program, programIndex) => {
                    return (
                        <div key={program.id} style={{ marginTop: "2rem" }}>
                            <div style={{ marginTop: "1rem" }}>
                                <DatePicker disabledDate={date => programe.some(program => program.date?.isSame(date, "day"))} onChange={(date) => updateProgramDate(programIndex, date)} />
                            </div>

                            <div style={{ marginTop: "1rem" }}>
                                <Select placeholder="Selecteaza un interval" style={{ minWidth: "160px" }} onChange={val => updateProgramTemplate(programIndex, val)}>
                                    {templates.map((template, idx) => {
                                        const startIndex = template.Config.indexOf("1")
                                        const endIndex = template.Config.lastIndexOf("1") + 1

                                        let text = ""

                                        if (endIndex - startIndex == 96) {
                                            text = "Whole day"
                                        }

                                        else {
                                            const startHour = moment(startOfDay).add(startIndex * 15, "minutes")
                                            const endHour = moment(startOfDay).add(endIndex * 15, "minutes")

                                            text = `${startHour.format("HH:mm")} - ${endHour.format("HH:mm")}`
                                        }

                                        return <Select.Option key={idx} value={template.Id}>{text}</Select.Option>
                                    })}
                                </Select>
                            </div>

                            <div style={{ marginTop: "1rem" }}>
                                <Button onClick={() => stergeProgram(programIndex)}>Sterge</Button>
                            </div>
                        </div>
                    )
                })}
            </div>

            <div style={{ marginBottom: "2rem" }}>
                <Button onClick={addProgram}>Adauga program</Button>
            </div>

            <Button disabled={programe.length === 0} onClick={onCreate}>Creeaza</Button>
        </div>
    )
}

export default DoctorProgramTemplateCreateComponent