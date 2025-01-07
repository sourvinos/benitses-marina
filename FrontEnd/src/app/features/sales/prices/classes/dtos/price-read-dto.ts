import { Metadata } from 'src/app/shared/classes/metadata'

export interface PriceReadDto extends Metadata {

    id: number
    code: string
    description: string
    englishDescription: string
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
