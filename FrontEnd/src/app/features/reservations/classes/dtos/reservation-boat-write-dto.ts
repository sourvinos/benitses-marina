import { Guid } from 'guid-typescript'

export interface ReservationBoatWriteDto {

    reservationId: Guid
    name: string
    flag: string
    loa: string
    beam: string
    draft: string
    registryPort: string
    registryNo: string
    isFishingBoat: boolean
    typeId: number
    usageId: number

}
