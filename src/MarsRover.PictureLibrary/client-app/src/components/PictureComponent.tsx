import * as React from 'react';
import PictureDTO from '../DTOs/PictureDTO';
import PictureService from '../services/PictureService';
import DatesService from '../services/DatesService';

interface State {
    currentPicture?: PictureDTO,
    currentPictureNo: number,
    selectedDate?: string,
    validDates?: string[],
    datesLoading: boolean,
    pictureLoading: boolean,
    pictureLoadError?: string
    dateLoadError?: string
}

export default class PictureComponent extends React.Component<{}, State>{
    constructor(props: {}) {
        super(props);
        this.state = {
            currentPicture: undefined,
            currentPictureNo: 0,
            selectedDate: undefined,
            validDates: undefined,
            datesLoading: false,
            pictureLoading: false,
            pictureLoadError: undefined,
            dateLoadError: undefined
        };
        this.onPictureNoChanged = this.onPictureNoChanged.bind(this);
        this.onSelectedDateChanged = this.onSelectedDateChanged.bind(this);
    }

    componentDidMount() {
        const service = new DatesService();
        const request = service.getValidDates();
        this.setState({ datesLoading: true, pictureLoading: true });
        request.then(result => {
            this.setState({ currentPictureNo: 1, validDates: result, selectedDate: result[0], datesLoading: false, dateLoadError: undefined });
            this.loadPicture();
        }).catch(error => {
            this.setState({ datesLoading: false, dateLoadError: error });
        });
    }

    loadPicture() {
        const service = new PictureService();
        if (this.state.selectedDate) {
            const request = service.getPicture(this.state.selectedDate, this.state.currentPictureNo);
            this.setState({ pictureLoading: true, pictureLoadError: undefined });
            request.then(result => {
                this.setState({ currentPicture: result, pictureLoading: false, pictureLoadError: undefined });
            }).catch(error => {
                this.setState({ currentPicture: undefined, pictureLoading: false, pictureLoadError: error });
            });
        }
    }

    onSelectedDateChanged(e: any) {
        this.setState(
            { 
                selectedDate: e.target.value, 
                currentPictureNo: 1, 
                currentPicture: undefined, 
                pictureLoading: false, 
                pictureLoadError: undefined 
            }, 
            () => { this.loadPicture(); }
        );
    }

    onPictureNoChanged(e: any) {
        if (e.target.value > 0) {
            this.setState({ currentPictureNo: e.target.value }, () => { this.loadPicture(); });
        }
    }

    render() {
        let selectArea: JSX.Element;
        let pictureArea: JSX.Element;
        let pictureNoInput: JSX.Element;

        if (this.state.datesLoading) {
            selectArea = <span>Loading...</span>;
        } else if (!this.state.dateLoadError && this.state.validDates) {
            selectArea = this.renderDatesSelect(this.state.validDates, this.state.selectedDate);
        } else {
            selectArea = <span>{this.state.dateLoadError}</span>;
        }

        pictureNoInput = (
            <label>
                Picture No :
                <input value={this.state.currentPictureNo} type='number' name="pictureNo" onChange={this.onPictureNoChanged}></input>
            </label>
        );

        if (this.state.pictureLoading) {
            pictureArea = (
                <div>
                    <img width="800" height="600" alt="Loading..."></img>
                </div>
            );
        } else if (!this.state.pictureLoadError && this.state.currentPicture) {
            pictureArea = (
                <div>
                    <img width="800" height="600" src={this.state.currentPicture.path} alt={this.state.currentPicture.id}></img>
                </div>
            );
        } else {
            pictureArea = (
                <div>
                    <img width="800" height="600" alt={this.state.pictureLoadError}></img>
                </div>
            );
        }


        return (
            <div>
                <form>
                    {selectArea}
                    <br />
                    <br />
                    {pictureNoInput}
                </form>
                <br />
                <br />
                {pictureArea}
            </div>);
    }

    renderPicture(picture: PictureDTO) {
        return (<img src={picture.path}></img>);
    }

    renderDatesSelect(validDates: string[], selectedDate: any) {
        const options = validDates.map((date) => <option key={date} value={date}>{date}</option>);
        return (
            <label> Select Date :
                <select value={selectedDate} onChange={this.onSelectedDateChanged}>
                    {options}
                </select>
            </label>
        );
    }
}