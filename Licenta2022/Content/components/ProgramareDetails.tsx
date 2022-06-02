import { Person } from "./types"

interface Programare {
    Pacient: Person

    Data: string

    Trimitere: {
        Id: number
    }

    Servicii: {
        Pret: number;
        Denumire: string
    }[]

    Retete: {
        Data: string;
        Medicamente: {
            Doza: number;
            Medicament: string
        }[]
    }[]
}

interface IProgramareDetails {
    programare: Programare
}

const ProgramareDetails: React.FC<IProgramareDetails> = ({ programare }) => {
    console.log({ programare })

    return <div>Hi</div>
}

export default ProgramareDetails
