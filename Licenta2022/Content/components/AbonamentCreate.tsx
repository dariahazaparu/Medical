import { Button, Divider, Form, Input, InputNumber, notification, Select } from "antd";
import { useForm } from "antd/lib/form/Form";
import axios from "axios";
import { some } from "lodash";
import { useState } from "react";
import { goToRoute } from "./utils";

interface Serviciu {
    id: number;
    nume: string;
}

interface IAbonamentCreate {
    servicii: Serviciu[]
}


const AbonamentCreate: React.FC<IAbonamentCreate> = ({ servicii }) => {
    const [usedServices, setUsedServices] = useState([])

    const [form] = useForm()

    const onSubmit = async (data: any) => {
        console.log(data)

        const { nume } = data

        const formServicii = data.servicii.map(serviciu => ({
            procentDiscount: Number(serviciu.procentDiscount),
            serviciuId: servicii.find(serv => serv.nume === serviciu.serviciuId).id
        }))

        try {
            const response = await axios.post("/Abonament/Create", {
                Nume: nume,
                IdServicii: formServicii.map(serviciu => serviciu.serviciuId),
                ProcenteDiscount: formServicii.map(serviciu => serviciu.procentDiscount)
            })

            if (response.status === 200) {
                goToRoute("/Abonament")
            }

        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }

    const availableServices = servicii.filter(serviciu => !usedServices.some(s => s === serviciu.nume))


    return (
        <div>
            <Form labelCol={{ span: 4, style: { marginRight: "1rem" } }} wrapperCol={{ span: 12 }} colon={false} form={form} onFinish={onSubmit} onValuesChange={(_, values) => setUsedServices(values.servicii.map(item => item.serviciuId))}>
                <Form.Item label="Nume" name="nume">
                    <Input placeholder="Nume" />
                </Form.Item>

                <Form.List initialValue={[{ serviciuId: null, procentDiscount: null }]} name="servicii">
                    {(fields, { add, remove }) => {
                        const onAdd = () => {
                            add({ serviciuId: null, procentDiscount: null })
                        }

                        return (
                            <div>
                                {fields.map((field, index) => {
                                    return <div key={field.key}>
                                        {index !== 0 && <Divider />}

                                        <Form.Item label="Serviciu" name={[index, "serviciuId"]}>
                                            <Select placeholder="Selecteaza un serviciu">
                                                {availableServices.map((serviciu, index) => (
                                                    <Select.Option key={index} title={serviciu.nume} value={serviciu.nume}>{serviciu.nume}</Select.Option>
                                                ))}
                                            </Select>
                                        </Form.Item>

                                        <Form.Item label="Procent discount" name={[index, "procentDiscount"]}>
                                            <InputNumber max={100} min={0} placeholder="Procent" />
                                        </Form.Item>

                                        {fields.length > 1 && <Button style={{ marginBottom: "1rem" }} onClick={() => remove(index)}>Șterge</Button>}

                                    </div>
                                })}

                                <Divider />

                                <Button style={{ justifyContent: "center" }} disabled={usedServices.length === servicii.length || form.getFieldValue("servicii").some(field => field.serviciuId == null)} onClick={onAdd}>Adaugă inca un serviciu</Button>

                            </div>
                        )
                    }}
                </Form.List>

                <Button style={{ marginTop: "1rem" }} type="primary" htmlType="submit">Creează</Button>
            </Form>
        </div>
    )
}

export default AbonamentCreate