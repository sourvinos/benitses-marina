import { Guid } from 'guid-typescript'

export interface ReservationBoatDto {

    reservationId: Guid
    name: string
    flag: string
    loa: string
    beam: string
    draft: string
    registryPort: string
    registryNo: string
    type: string
    usage: string
}
