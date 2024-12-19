export interface SupplierWriteDto {

    // PK
    id: number
    // Fields
    description: string
    vatNumber: string
    phones: string
    email: string
    remarks: string
    isActive: boolean
    // Metadata
    putAt: string

}
