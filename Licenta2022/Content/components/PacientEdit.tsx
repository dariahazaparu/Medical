import { Button, Form, Input, notification, Row, Select } from "antd"
import { useForm } from "antd/lib/form/Form";
import axios from "axios";
import { useState } from "react";
import { DropdownItem, Person } from "./types"
import { goToRoute } from "./utils";

interface Pacient extends Person {
    Id: string;

    CNP: string;

    IdAdresa: number;
    IdLocalitate: number;
    IdAsigurare: number;
}

interface Adresa {
    label: string;
    value: any;

    localitateId: any;
}

interface IPacientEdit {
    data: Pacient

    localitati: DropdownItem[];
    adrese: Adresa[];
    asigurari: DropdownItem[];
}

const PacientEdit: React.FC<IPacientEdit> = ({ data, localitati, adrese, asigurari }) => {
    if (!data) return null;

    const [localitateId, setLocalitateId] = useState(data?.IdLocalitate)

    const [form] = useForm()

    const onSubmit = async (formValues: any) => {
        try {
            const response = await axios.post(`/Pacient/Edit/${data.Id}`, formValues)

            if (response.status === 200) {
                goToRoute(`/Pacient/Details/${data.Id}`)
            }
        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }


    const onLocalitateChange = (value: any) => {
        setLocalitateId(value)

        form.setFieldsValue({
            IdAdresa: adrese.filter(adresa => adresa.localitateId === value)[0].value
        })
    }

    return (
        <Form labelCol={{ span: 2 }} wrapperCol={{ span: 12 }} colon={false} form={form} initialValues={data} onFinish={onSubmit}>

            <Form.Item label="Nume" name="Nume">
                <Input />
            </Form.Item>

            <Form.Item label="Prenume" name="Prenume">
                <Input />
            </Form.Item>

            <Form.Item label="CNP" name="CNP">
                <Input />
            </Form.Item>

            <Form.Item label="Localitate">
                <Select defaultValue={localitati.find(localitate => localitate.value === localitateId).value} onChange={value => onLocalitateChange(value)} options={localitati} />
            </Form.Item>

            <Form.Item label="Adresa" name="IdAdresa">
                <Select>
                    {adrese.filter(adresa => adresa.localitateId === localitateId).map((adresa) => (
                        <Select.Option key={adresa.value} value={adresa.value}>{adresa.label}</Select.Option>
                    ))}
                </Select>
            </Form.Item>

            <Form.Item label="Asigurare" name="IdAsigurare">
                <Select options={asigurari} />
            </Form.Item>

            <Button htmlType="submit" type="primary">Editeaza</Button>
        </Form>
    )
}

export default PacientEdit
