import { Guid } from 'guid-typescript'

export interface PierReadDto {

    pierId: number
    reservationId: Guid
    description: string

}