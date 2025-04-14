export class LoginRequestDto {
    userName: string = "";
    password: string = "";
    
}

export interface AuthResponse{
    jwtToken : string;
}