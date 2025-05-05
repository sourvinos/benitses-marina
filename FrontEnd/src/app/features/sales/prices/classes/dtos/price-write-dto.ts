export interface PriceWriteDto {

    id: number
    hullTypeId: number
    periodTypeId: number
    seasonTypeId: number
    code: string
    description: string
    englishDescription: string
    isIndividual: boolean
    length: number
    netAmount: number
    vatPercent: number
    putAt: string

}
