export interface SupplierWriteDto {

    id: number
    bankId: number
    iban: string
    description: string
    longDescription: string
    vatNumber: string
    phones: string
    email: string
    remarks: string
    isActive: boolean
    putAt: string

}
