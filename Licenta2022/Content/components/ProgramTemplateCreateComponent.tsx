import { useState } from 'react'

import axios from 'axios'

import dayjs from 'dayjs'

import { DatePicker, List } from 'antd'

import Moment from 'moment'

import { useHistory } from 'react-router-dom'

// import 'antd/dist/antd.css'

const ProgramTemplateCreateComponent = (props: any) => {
    const [date, setDate] = useState(Moment());
    const [beginningOfDay, _] = useState(dayjs().startOf("day"))

    const [config, setConfig] = useState(new Array(96).fill(null).map(v => false))

    const onSubmit = async (e: any) => {
        e.preventDefault()

        const response = await axios.post("/ProgramTemplate/Create", {
            Config: config,
            Data: date,
        })

        if (response.status === 200) {
            window.location.href = "/ProgramTemplate"
        }
    }

    const updateConfig = (index: number, checked: boolean) => {
        setConfig(prev => {
            prev[index] = checked

            return prev
        })
    }

    return (
        <div>
            {/* <DatePicker value={date} onChange={date => setDate(date)} /> */}
            <form onSubmit={onSubmit} action="/ProgramTemplate/Create" method="post">
                <List style={{ maxHeight: "400px", overflow: "scroll" }}>
                    {new Array(96).fill(null).map((_, idx) => {
                        const fromDate = beginningOfDay.add(idx * 15, "minutes")
                        const toDate = beginningOfDay.add((idx + 1) * 15, "minutes")

                        return (
                            <List.Item key={idx} style={{ display: "flex" }}>
                                <div>
                                    {fromDate.format("HH:mm:ss")} - {toDate.format("HH:mm:ss")}
                                </div>

                                <div>
                                    <input type="checkbox" onChange={e => updateConfig(idx, e.target.checked)} />
                                </div>
                            </List.Item>
                        )
                    })}
                </List>

                <button type="submit">Create</button>
            </form>
        </div>
    )

}

export default ProgramTemplateCreateComponent
