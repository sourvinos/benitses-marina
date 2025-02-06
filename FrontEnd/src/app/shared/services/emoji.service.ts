import { Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })

export class EmojiService {

    public getEmoji(emoji: string): string {
        switch (emoji) {
            case 'green-box': return '🟩'
            case 'red-box': return '🟥'
            case 'wildcard': return '⭐'
            case 'empty': return ''
        }

    }

}
