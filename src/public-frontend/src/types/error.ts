import { array, mapping, string } from 'decoders';

export type GenericErrors = Map<string, string[]>;

export const genericErrorsDecoder = mapping(array(string));
