import { commands, ICommand } from '@uiw/react-md-editor';

export const getExtraCommands: () => ICommand[] = () => [
  commands.codeEdit,
  commands.codeLive,
  commands.codePreview,
  commands.divider,
  commands.fullscreen,
];
