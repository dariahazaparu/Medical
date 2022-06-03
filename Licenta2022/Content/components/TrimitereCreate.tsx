import { Button, Form, Input, notification, Select } from "antd"
import { useForm } from "antd/lib/form/Form"
import TextArea from "antd/lib/input/TextArea"
import axios from "axios"
import { useState } from "react"
import { DropdownItem } from "./types"
import { goToRoute } from "./utils"

interface ITrimitere {
    specialitati: DropdownItem[]
    servicii: (DropdownItem & { specializareId: number })[]

    idPacient: number
    idProgramare: number
}

const TrimitereCreate: React.FC<ITrimitere> = ({ specialitati, servicii, idPacient, idProgramare }) => {
    const [form] = useForm()

    const [specializareId, setSpecializareId] = useState(null)
    const [serviciiId, setServiciiId] = useState([])

    const serviciiAvailable = specializareId ? servicii.filter(serviciu => serviciu.specializareId === specializareId) : []

    const onSubmit = async (data: any) => {
        const { observatii, specializareId, serviciiId } = data

        try {
            const response = await axios.post("/Trimitere/Create", {
                Observatii: observatii,
                IdProgramare: idProgramare,
                IdPacient: idPacient,
                IdSpecializare: specializareId,
                IdServicii: serviciiId
            })

            if (response.status === 200) {
                goToRoute(`/Programare/Details/${idProgramare}`)
            }
        }

        catch (err) {
            notification.error({ message: err.request.statusText })
        }
    }

    const updateSpecializareId = (id: any) => {
        setSpecializareId(id)
        setServiciiId([])

        form.setFieldsValue({
            serviciiId: []
        })
    }

    return (
        <div>
            <Form form={form} labelCol={{ span: 2 }} wrapperCol={{ span: 12 }} colon={false} onFinish={onSubmit}>
                <Form.Item label="Specializare" name="specializareId">
                    <Select onChange={updateSpecializareId} placeholder="Selecteaza o specializare" options={specialitati} />
                </Form.Item>


                <Form.Item label="Servicii" name="serviciiId">
                    <Select onChange={setServiciiId} allowClear mode="multiple" placeholder="Selecteaza servicii" options={serviciiAvailable} />
                </Form.Item>


                <Form.Item label="Observatii" name="observatii">
                    <TextArea />
                </Form.Item>

                <Button disabled={serviciiId.length === 0} htmlType="submit" type="primary">Creeaza</Button>
            </Form>
        </div>
    )
}

export default TrimitereCreate
