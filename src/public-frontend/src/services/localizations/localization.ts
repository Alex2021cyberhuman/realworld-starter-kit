import { format } from 'date-fns';
import { ru, enGB } from 'date-fns/locale';
import { enStrings, Localization } from './enStrings';
import { ruStrings } from './ruStrings';
import { useStore } from '../../state/storeHooks';
import LocalizedStrings from 'react-localization';

export function useLocalization(): { localization: Localization; language: string } {
  const { localization, language } = useStore(({ app }) => app);
  return { localization, language };
}

export const languagesDictionaries = new Map<string, Localization>([
  ['en', enStrings],
  ['ru', ruStrings],
]);

export const languagesTranslates = new Map<string, string>([
  ['en', 'English'],
  ['ru', 'Русский'],
]);

const dateFnsLocales = new Map<string, Locale>([
  ['ru', ru],
  ['en', enGB],
]);

export function getLocalDate(date: Date, language?: string | undefined) {
  const newDate = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
  const finalLanguage = language || localStorage.getItem('languageCode') || 'en';
  const locale = dateFnsLocales.get(finalLanguage);
  return format(newDate, 'PPPP', { locale });
}

const fakeLocalization = new LocalizedStrings({ fake: { fake1: 'fake1' } });

type Formatted = number | string | JSX.Element;
type FormatObject<U extends Formatted> = { [key: string]: U };

export function formatString<T extends Formatted>(
  str: string,
  ...values: Array<T | FormatObject<T>>
): Array<string | T> | string {
  return fakeLocalization.formatString<T>(str, ...values);
}
