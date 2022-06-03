import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import ProgramTemplateCreateComponent from "./ProgramTemplateCreateComponent.tsx"
import DoctorProgramTemplateCreateComponent from "./DoctorProgramTemplateCreateComponent.tsx"
import ProgramareCreateComponent from "./ProgramareCreateComponent.tsx"

import PacientTable from "./PacientTable.tsx"
import ProgramareTable from "./ProgramareTable"
import TrimitereTable from "./TrimitereTable"
import DoctorTable from "./DoctorTable"
import ClinicaTable from "./ClinicaTable"

import TrimitereCreate from "./TrimitereCreate"
import RetetaCreate from "./RetetaCreate"

import ProgramareDetails from "./ProgramareDetails"

import PacientEdit from "./PacientEdit"

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Components = {
    ProgramTemplateCreateComponent, DoctorProgramTemplateCreateComponent, ProgramareCreateComponent,
    PacientTable, ProgramareTable, TrimitereTable, DoctorTable, ClinicaTable, PacientEdit, ProgramareDetails,
    TrimitereCreate, RetetaCreate
};

