import { useState } from "react";

import axios from 'axios'
import { DatePicker, TimePicker } from "antd";
import moment from "moment";
import 'antd/dist/antd.css'

const ProgramTemplateCreateComponent = (props: any) => {
    // const [date, setDate] = useState(Moment());
    // const [beginningOfDay, _] = useState(dayjs().startOf("day"))

    // const [config, setConfig] = useState(new Array(96).fill(null).map(v => false))

    // const onSubmit = async (e: any) => {
    //     e.preventDefault()

    //     const response = await axios.post("/ProgramTemplate/Create", {
    //         Config: config,
    //         Data: date,
    //     })

    //     if (response.status === 200) {
    //         window.location.href = "/ProgramTemplate"
    //     }
    // }

    // const updateConfig = (index: number, checked: boolean) => {
    //     setConfig(prev => {
    //         prev[index] = checked

    //         return prev
    //     })
    // }

    // return (
    //     <div>
    //         {/* <DatePicker value={date} onChange={date => setDate(date)} /> */}
    //         <form onSubmit={onSubmit}>

    //             {/* <List style={{ maxHeight: "400px", overflow: "scroll" }}>
    //                 {new Array(96).fill(null).map((_, idx) => {
    //                     const fromDate = beginningOfDay.add(idx * 15, "minutes")
    //                     const toDate = beginningOfDay.add((idx + 1) * 15, "minutes")

    //                     return (
    //                         <List.Item key={idx} style={{ display: "flex" }}>
    //                             <div>
    //                                 {fromDate.format("HH:mm")} - {toDate.format("HH:mm")}
    //                             </div>

    //                             <div>
    //                                 <input type="checkbox" onChange={e => updateConfig(idx, e.target.checked)} />
    //                             </div>
    //                         </List.Item>
    //                     )
    //                 })}
    //             </List> */}

    //             <button type="submit">Create</button>
    //         </form>
    //     </div>
    // )


 const [date, setDate] = useState(moment());
  const [startHour, setStartHour] = useState(null);
  const [endHour, setEndHour] = useState(null);

  const [isWholeDay, setIsWholeDay] = useState(false);

  const updateStartHour = (date) => {
    setStartHour(date);

    if (date) {
      if (!endHour || date.isAfter(endHour)) {
        setEndHour(date.add(15, "minutes"));
      }
    } else {
      setEndHour(null);
    }
  };

  const onSubmit = () => {
    const startIndex =
      (startHour.get("hours") * 60 + startHour.get("minutes")) / 15;
    const endIndex = (endHour.get("hours") * 60 + endHour.get("minutes")) / 15;

    const answer = new Array(96)
      .fill(null)
      .map((_, idx) => isWholeDay || (idx >= startIndex && idx < endIndex));

    console.log({ answer });
  };

  return (
    <div className="App">
      <div>
        <DatePicker value={date} onChange={setDate} />
      </div>

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

      <button disabled={!startHour} onClick={onSubmit}>
        Create
      </button>
    </div>
  );
}

export default ProgramTemplateCreateComponent
