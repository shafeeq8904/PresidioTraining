export interface User {
  id: number;
  firstName: string;
  lastName: string;
  image:string;
  gender: 'male' | 'female';
  role: string; // mapped to readable string
  state?: string; // extracted from address
}
