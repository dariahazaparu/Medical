import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import ProgramTemplateCreateComponent from "./ProgramTemplateCreateComponent.tsx"
import DoctorProgramTemplateCreateComponent from "./DoctorProgramTemplateCreateComponent.tsx"
import ProgramareCreateComponent from "./ProgramareCreateComponent.tsx"
import PacientTable from "./PacientTable.tsx"
import ProgramareTable from "./ProgramareTable"

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Components = { ProgramTemplateCreateComponent, DoctorProgramTemplateCreateComponent, ProgramareCreateComponent, PacientTable, ProgramareTable };
