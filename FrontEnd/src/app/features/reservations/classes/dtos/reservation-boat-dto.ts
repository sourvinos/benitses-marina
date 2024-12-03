import { Guid } from 'guid-typescript'
// Custom
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationBoatDto {

    reservationId: Guid
    name: string
    flag: string
    loa: string
    beam: string
    draft: string
    registryPort: string
    registryNo: string
    type: SimpleEntity
    usage: SimpleEntity

}
