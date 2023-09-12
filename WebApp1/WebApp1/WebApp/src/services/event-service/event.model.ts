export enum EUserType{
  HighSchool,
  Student
}
export interface Trainer{
  firstName :string;
  lastName:string;
  email:string;
}

export interface Country {
  countryId: number
  name: string
}

export interface IEvent{
  eventId : number
  name : string
  location : string
  country : string
  onlineMeetingUrl : string
  freePlaces : boolean
  eventDateTime:string
  numberOfParticipants:number,
  numberOfRegisteredParticipants:number,
  description : string
  currentUserAlreadyOnEvent :boolean
  eventUserType : EUserType
  trainers:Trainer[]
  usersOnEvent: UserOnEvent[]
  start:Date
}

export interface CreateEvent{
  name : string
  location : string
  onlineMeetingUrl : string
  freePlaces : boolean
  numberOfFreePlaces :number
  description : string
  eventForUserType : EUserType
  trainerIds: number[]
  eventDateTime:string
}

export interface UpdateEventDto{
  name : string
  location : string
  onlineMeetingUrl : string
  freePlaces : boolean
  numberOfFreePlaces :number
  description : string
  eventForUserType : EUserType
  trainerIds: number[]
  eventDateTime:string
}
export interface CreateTrainer{
  firstName : string
  lastName : string
  email : string
  phoneNumber : string
  countryId : number
}


export interface UpdateTrainer{
  id?:number;
  firstName : string
  lastName : string
  email : string
  phoneNumber : string
  countryId : number
}

export interface EventPanelSubjectModel{
  events:IEvent[]
  rightPanelOpened:boolean
  selectedDate:Date | null
}

export interface UserOnEvent{
  userId: string
  firstName : string
  lastName : string
  email : string
}

