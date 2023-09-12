import { Pipe } from '@angular/core';

@Pipe({ name: 'preview' })
export class EventPipe {
  transform(array: any, selectedDate:boolean): any[] {
    if(!selectedDate)
      return array.filter((e:any,i:number)=>i == 0);
    return array;
  }
}
