export interface CustomerWriteDto {

    id: number
    nationalityId: number
    taxOfficeId: number
    vatPercent: number
    vatPercentId: number
    vatExemptionId: number
    description: string
    fullDescription: string
    vatNumber: string
    branch: number
    profession: string
    street: string
    number: string
    postalCode: string
    city: string
    personInCharge: string
    phones: string
    email: string
    remarks: string
    isActive: boolean
    putAt: string

}
