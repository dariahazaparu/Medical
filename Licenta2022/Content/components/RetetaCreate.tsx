import { Button, DatePicker, Divider, Form, Input, notification, Select } from "antd"
import { useForm } from "antd/lib/form/Form"
import axios from "axios"
import moment from "moment"
import { useState } from "react"
import { DropdownItem } from "./types"
import { goToRoute } from "./utils"

interface IRetetaCreate {
    medicamente: DropdownItem[]
    programareId: number;
}

const RetetaCreate: React.FC<IRetetaCreate> = ({ medicamente, programareId }) => {
    const [usedMedicines, setUsedMedicines] = useState([])

    const [form] = useForm()

    const onSubmit = async (data: any) => {
        const formMedicamente = data.medicamente.map(medicament => ({
            ...medicament,
            medicamentId: medicamente.find(med => med.label === medicament.medicamentId).value
        }))

        try {
            const response = await axios.post("/Reteta/Create", {
                IdProgramare: programareId,
                IdMedicamente: formMedicamente.map(item => item.medicamentId),
                Doze: formMedicamente.map(item => item.doza)
            })

            if (response.status === 200) {
                goToRoute(`/Programare/Details/${programareId}`)
            }
        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }

    const availableMedicines = medicamente.filter(medicament => !usedMedicines.some(id => id === medicament.label))

    return (
        <div>
            <Form onValuesChange={(_, values) => {
                console.log(values)
                setUsedMedicines(values.medicamente.map(item => item.medicamentId))
            }} form={form} onFinish={onSubmit}>
                <Form.List initialValue={[{ medicamentId: null, doza: null }]} name="medicamente">
                    {(fields, { add, remove }) => {

                        const onAdd = () => {
                            add({ medicamentId: null, doza: null })
                        }

                        return (
                            <div>
                                {fields.map((field, index) => {
                                    return <div key={field.key}>
                                        {index !== 0 && <Divider />}

                                        <Form.Item name={[index, "medicamentId"]}>
                                            <Select placeholder="Selecteaza un medicament">
                                                {availableMedicines.map((medicine, index) => (
                                                    <Select.Option key={index} title={medicine.label} value={medicine.label}>{medicine.label}</Select.Option>
                                                ))}
                                            </Select>
                                        </Form.Item>

                                        <Form.Item rules={[{ required: true, message: "Campul trebuie completat." }]} name={[index, "doza"]}>
                                            <Input placeholder="Doza" />
                                        </Form.Item>

                                        {fields.length > 1 && <Button style={{ marginBottom: "1rem" }} onClick={() => remove(index)}>Sterge</Button>}
                                    </div>
                                })}

                                <Button disabled={usedMedicines.length === medicamente.length || form.getFieldValue("medicamente").some(field => field.medicamentId === null)} onClick={onAdd}>Adauga inca un medicament</Button>
                            </div>
                        )
                    }}
                </Form.List>

                <Button style={{ marginTop: "1rem" }} type="primary" htmlType="submit">Creeaza</Button>
            </Form>
        </div>
    )
}

export default RetetaCreate
