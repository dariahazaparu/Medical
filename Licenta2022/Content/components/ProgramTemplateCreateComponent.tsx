import { useState } from "react";

import axios from 'axios'
import { DatePicker, notification, TimePicker } from "antd";
import moment, { Moment } from "moment";

import 'antd/dist/antd.css';

const OldComponent = () => {
    const [date, setDate] = useState(moment());
    const [beginningOfDay, _] = useState(moment().startOf("day"))

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
            <form onSubmit={onSubmit}>

                {/* <List style={{ maxHeight: "400px", overflow: "scroll" }}>
                    {new Array(96).fill(null).map((_, idx) => {
                        const fromDate = beginningOfDay.add(idx * 15, "minutes")
                        const toDate = beginningOfDay.add((idx + 1) * 15, "minutes")

                        return (
                            <List.Item key={idx} style={{ display: "flex" }}>
                                <div>
                                    {fromDate.format("HH:mm")} - {toDate.format("HH:mm")}
                                </div>

                                <div>
                                    <input type="checkbox" onChange={e => updateConfig(idx, e.target.checked)} />
                                </div>
                            </List.Item>
                        )
                    })}
                </List> */}

                <button type="submit">Create</button>
            </form>
        </div>
    )
}

const ProgramTemplateCreateComponent = (props: any) => {
    const [startHour, setStartHour] = useState(null);
    const [endHour, setEndHour] = useState(null);

    const [isWholeDay, setIsWholeDay] = useState(false);

    const updateStartHour = (date: Moment) => {
        setStartHour(date);

        if (date) {
            if (!endHour || date.isAfter(endHour)) {
                setEndHour(date.add(15, "minutes"));
            }
        } else {
            setEndHour(null);
        }
    };

    const onSubmit = async () => {
        let config = null

        if (isWholeDay) {
            config = "1".repeat(96)
        }

        else {
            const startIndex =
                (startHour.get("hours") * 60 + startHour.get("minutes")) / 15;
            const endIndex = (endHour.get("hours") * 60 + endHour.get("minutes")) / 15;

            config = new Array(96).fill(null).map((_, idx) => Number((idx >= startIndex && idx < endIndex))).join("")
        }

        const response = await axios.post("/ProgramTemplate/Create", {
            Config: config,
        })

        if (response.status === 200) {
            window.location.href = "/ProgramTemplate"
        }

        // else {
        //     notification.info({message: response.})
        // }
    };

    return (
        <div className="App">
            <div style={{ marginTop: "1rem" }}>
                <label>Whole day?</label>
                <input
                    style={{ marginLeft: "1rem" }}
                    type="checkbox"
                    onChange={(e) => setIsWholeDay(e.target.checked)}
                />
            </div>

            <div
                style={{
                    display: "flex",
                    justifyContent: "space-around",
                    marginTop: "2rem"
                }}
            >
                <TimePicker
                    disabled={isWholeDay}
                    format="HH:mm"
                    minuteStep={15}
                    value={startHour}
                    onChange={updateStartHour}
                />

                <TimePicker
                    disabled={isWholeDay}
                    format="HH:mm"
                    minuteStep={15}
                    value={endHour}
                    onChange={setEndHour}
                    disabledDate={(date) => !startHour || date.isSameOrBefore(startHour)}
                />
            </div>

            <button disabled={!startHour && !isWholeDay} onClick={onSubmit}>
                Create
            </button>
        </div>
    );
}

export default ProgramTemplateCreateComponent
