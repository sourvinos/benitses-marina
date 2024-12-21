import { Guid } from 'guid-typescript'

export interface BerthReadDto {

    berthId: number
    reservationId: Guid
    description: string

}